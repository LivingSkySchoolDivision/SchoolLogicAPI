using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.WebSockets;
using SchoolLogicAPI.Models;

namespace SchoolLogicAPI.Repositories
{
    public class ReportPeriodRepository
    {
        private const string SQLQueryStart = "SELECT ReportPeriod.iReportPeriodID, ReportPeriod.dStartDate, ReportPeriod.dEndDate, ReportPeriod.iTermID, ReportPeriod.cName, ReportPeriod.mComment, ReportPeriod.lPostMarks, School.cCode, School.iSchoolID FROM Track LEFT OUTER JOIN School ON Track.iSchoolID = School.iSchoolID RIGHT OUTER JOIN Term ON Track.iTrackID = Term.iTrackID RIGHT OUTER JOIN ReportPeriod ON Term.iTermID = ReportPeriod.iTermID";

        private static ReportPeriod sqlDataReaderToReportPeriod(SqlDataReader dataReader)
        {
            return new ReportPeriod()
            {
                ID = Parsers.ParseInt(dataReader["iReportPeriodID"].ToString().Trim()),
                Name = dataReader["cName"].ToString().Trim(),
                StartDate =Parsers.ParseDate(dataReader["dStartDate"].ToString()),
                EndDate = Parsers.ParseDate(dataReader["dEndDate"].ToString()).AddDays(1).AddMinutes(-1),
                SchoolInternalId = Parsers.ParseInt(dataReader["iSchoolID"].ToString().Trim()),
                SchoolGovId = Parsers.ParseInt(dataReader["cCode"].ToString().Trim()),
                TermId = Parsers.ParseInt(dataReader["iTermID"].ToString().Trim())
            };
        }

        private static Dictionary<int, Term> _allTerms = new Dictionary<int, Term>(); 
        private static Dictionary<int, Track> _allTracks = new Dictionary<int, Track>();
        private Dictionary<int, Dictionary<string, string>> _schoolSettingsBySchool = new Dictionary<int, Dictionary<string, string>>();

        private readonly object _cacheLockObject = new object();
        private DateTime _cacheLastRefreshed = DateTime.MinValue;

        private ReportPeriod LoadAdditionalReportPeriodData(ReportPeriod rp)
        {
            lock (_cacheLockObject)
            {
                if (DateTime.Now.Subtract(_cacheLastRefreshed) > new TimeSpan(0, 5, 0))
                {
                    _allTerms = new Dictionary<int, Term>();
                    _allTracks = new Dictionary<int, Track>();
                    _schoolSettingsBySchool = new Dictionary<int, Dictionary<string, string>>();

                    TermRepository termRepository = new TermRepository();
                    TrackRepository trackRepository = new TrackRepository();
                    SchoolSettingsRepository settingsRepository = new SchoolSettingsRepository();

                    _cacheLastRefreshed = DateTime.Now;


                    // School settings (for days open before and after)
                    // Split into a dictionary by school ID
                    foreach (SchoolSetting setting in settingsRepository.GetAll())
                    {
                        if (!_schoolSettingsBySchool.ContainsKey(setting.SchoolDatabaseID))
                        {
                            _schoolSettingsBySchool.Add(setting.SchoolDatabaseID, new Dictionary<string, string>());
                        }
                        _schoolSettingsBySchool[setting.SchoolDatabaseID].Add(setting.Key, setting.Value);
                    }

                    // Tracks
                    foreach (Track track in trackRepository.GetAll())
                    {
                        if (!_allTracks.ContainsKey(track.ID))
                        {
                            _allTracks.Add(track.ID, track);
                        }
                    }

                    // Terms
                    foreach (Term term in termRepository.GetAll())
                    {
                        if (!_allTerms.ContainsKey(term.ID))
                        {
                            _allTerms.Add(term.ID, term);
                        }
                    }

                }

                // Add additional information from the school's settings
                if (_schoolSettingsBySchool.ContainsKey(rp.SchoolInternalId))
                {
                    if (_schoolSettingsBySchool[rp.SchoolInternalId].ContainsKey("Grades/PreReportDays"))
                    {
                        rp.DaysOpenBeforeEnd =
                            Parsers.ParseInt(_schoolSettingsBySchool[rp.SchoolInternalId]["Grades/PreReportDays"]);
                    }

                    if (_schoolSettingsBySchool[rp.SchoolInternalId].ContainsKey("Grades/PostReportDays"))
                    {
                        rp.DaysOpenAfterEnd =
                            Parsers.ParseInt(_schoolSettingsBySchool[rp.SchoolInternalId]["Grades/PostReportDays"]);
                    }
                }

                // Add track ID
                if (_allTerms.ContainsKey(rp.TermId))
                {
                    rp.TrackID = _allTerms[rp.TermId].TrackID;
                }

                // Add the "full name", including track and term name
                rp.FullName = rp.Name;

                if (_allTerms.ContainsKey(rp.TermId))
                {
                    rp.FullName = _allTerms[rp.TermId].Name + " - " + rp.FullName;
                }

                if (_allTracks.ContainsKey(rp.TrackID))
                {
                    rp.FullName = _allTracks[rp.TrackID].Name + " - " + rp.FullName;
                }

            }

            return rp;
        }


        public List<ReportPeriod> GetAll()
        {
            List<ReportPeriod> workingReportPeriodList = new List<ReportPeriod>();

            using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQLQueryStart
                };
                sqlCommand.Connection.Open();
                SqlDataReader dataReader = sqlCommand.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ReportPeriod parsedReportPeriod = sqlDataReaderToReportPeriod(dataReader);
                        if (parsedReportPeriod != null)
                        {
                            workingReportPeriodList.Add(parsedReportPeriod);
                        }
                    }
                }
                sqlCommand.Connection.Close();
            }

            List<ReportPeriod> returnMe = new List<ReportPeriod>();

            foreach (ReportPeriod rp in workingReportPeriodList)
            {
                returnMe.Add(LoadAdditionalReportPeriodData(rp));
            }
            
            return returnMe;
            
        }

        public List<ReportPeriod> GetBySchool(int schoolDatabaseID)
        {
            List<ReportPeriod> workingReportPeriodList = new List<ReportPeriod>();

            using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
            {
                // Now we can load the actual report periods
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQLQueryStart + " WHERE ReportPeriod.iSchoolID=@SCHOOLID;"
                };
                sqlCommand.Parameters.AddWithValue("SCHOOLID", schoolDatabaseID);
                sqlCommand.Connection.Open();
                SqlDataReader dataReader = sqlCommand.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ReportPeriod parsedReportPeriod = sqlDataReaderToReportPeriod(dataReader);
                        if (parsedReportPeriod != null)
                        {
                            workingReportPeriodList.Add(parsedReportPeriod);
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }

            List<ReportPeriod> returnMe = new List<ReportPeriod>();

            foreach (ReportPeriod rp in workingReportPeriodList)
            {
                returnMe.Add(LoadAdditionalReportPeriodData(rp));
            }

            return returnMe;
        }

        public ReportPeriod Get(int reportPeriodID)
        {
            ReportPeriod returnMe = null;

            using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
            {
                // Now we can load the actual report periods
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQLQueryStart + " WHERE ReportPeriod.iReportPeriodID=@REPORTPERIODID;"
                };
                sqlCommand.Parameters.AddWithValue("REPORTPERIODID", reportPeriodID);
                sqlCommand.Connection.Open();
                SqlDataReader dataReader = sqlCommand.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ReportPeriod parsedReportPeriod = sqlDataReaderToReportPeriod(dataReader);
                        if (parsedReportPeriod != null)
                        {
                            returnMe = parsedReportPeriod;
                        }
                    }
                }

                sqlCommand.Connection.Close();
                
                return LoadAdditionalReportPeriodData(returnMe);
            }
        }
    }
}