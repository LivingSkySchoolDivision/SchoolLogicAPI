using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Profile;
using SchoolLogicAPI.Models;

namespace SchoolLogicAPI.Repositories
{
    public class ReportPeriodRepository
    {
        private static ReportPeriod sqlDataReaderToReportPeriod(SqlDataReader dataReader)
        {
            return new ReportPeriod()
            {
                ID = Parsers.ParseInt(dataReader["iReportPeriodID"].ToString().Trim()),
                Name = dataReader["cName"].ToString().Trim(),
                StartDate =Parsers.ParseDate(dataReader["dStartDate"].ToString()),
                EndDate = Parsers.ParseDate(dataReader["dEndDate"].ToString()),
                SchoolInternalId = Parsers.ParseInt(dataReader["iSchoolID"].ToString().Trim()),
                SchoolGovId = Parsers.ParseInt(dataReader["cCode"].ToString().Trim()),
                TermId = Parsers.ParseInt(dataReader["iTermID"].ToString().Trim())
            };
        }
        

        public List<ReportPeriod> GetAll()
        {
            List<ReportPeriod> returnMe = new List<ReportPeriod>();

            using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
            {
                // We'll need a list of the school's settings to see when the report periods open and close
                // Organize this list by school
                Dictionary<int, Dictionary<string, string>> schoolSettingsBySchool = new Dictionary<int, Dictionary<string, string>>();
                SchoolSettingsRepository settingsrepository = new SchoolSettingsRepository();
                foreach (SchoolSetting setting in settingsrepository.GetAll())
                {
                    if (!schoolSettingsBySchool.ContainsKey(setting.SchoolDatabaseID))
                    {
                        schoolSettingsBySchool.Add(setting.SchoolDatabaseID, new Dictionary<string, string>());
                    }
                    schoolSettingsBySchool[setting.SchoolDatabaseID].Add(setting.Key, setting.Value);
                }
                
                // Now we can load the actual report periods
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT ReportPeriod.iReportPeriodID, ReportPeriod.dStartDate, ReportPeriod.dEndDate, ReportPeriod.iTermID, ReportPeriod.cName, ReportPeriod.mComment, ReportPeriod.lPostMarks, ReportPeriod.iSchoolID, School.cCode FROM ReportPeriod LEFT OUTER JOIN School ON ReportPeriod.iSchoolID = School.iSchoolID;"
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
                            returnMe.Add(parsedReportPeriod);
                        }
                    }
                }

                sqlCommand.Connection.Close();


                // Add additional information from the school's settings
                foreach (ReportPeriod rp in returnMe)
                {
                    if (schoolSettingsBySchool.ContainsKey(rp.SchoolInternalId))
                    {
                        if (schoolSettingsBySchool[rp.SchoolInternalId].ContainsKey("Grades/PreReportDays"))
                        {
                            rp.DaysOpenBeforeEnd = Parsers.ParseInt(schoolSettingsBySchool[rp.SchoolInternalId]["Grades/PreReportDays"]);
                        }

                        if (schoolSettingsBySchool[rp.SchoolInternalId].ContainsKey("Grades/PostReportDays"))
                        {
                            rp.DaysOpenAfterEnd = Parsers.ParseInt(schoolSettingsBySchool[rp.SchoolInternalId]["Grades/PostReportDays"]);
                        }
                    }
                }
                return returnMe;
            }
        }

        public List<ReportPeriod> GetBySchool(int schoolDatabaseID)
        {
            List<ReportPeriod> returnMe = new List<ReportPeriod>();

            using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
            {
                // We'll need a list of the school's settings to see when the report periods open and close
                // Organize this list by school
                Dictionary<int, Dictionary<string, string>> schoolSettingsBySchool = new Dictionary<int, Dictionary<string, string>>();
                SchoolSettingsRepository settingsrepository = new SchoolSettingsRepository();
                foreach (SchoolSetting setting in settingsrepository.GetAll())
                {
                    if (!schoolSettingsBySchool.ContainsKey(setting.SchoolDatabaseID))
                    {
                        schoolSettingsBySchool.Add(setting.SchoolDatabaseID, new Dictionary<string, string>());
                    }
                    schoolSettingsBySchool[setting.SchoolDatabaseID].Add(setting.Key, setting.Value);
                }

                // Now we can load the actual report periods
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT ReportPeriod.iReportPeriodID, ReportPeriod.dStartDate, ReportPeriod.dEndDate, ReportPeriod.iTermID, ReportPeriod.cName, ReportPeriod.mComment, ReportPeriod.lPostMarks, ReportPeriod.iSchoolID, School.cCode FROM ReportPeriod LEFT OUTER JOIN School ON ReportPeriod.iSchoolID = School.iSchoolID WHERE ReportPeriod.iSchoolID=@SCHOOLID;"
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
                            returnMe.Add(parsedReportPeriod);
                        }
                    }
                }

                sqlCommand.Connection.Close();


                // Add additional information from the school's settings
                foreach (ReportPeriod rp in returnMe)
                {
                    if (schoolSettingsBySchool.ContainsKey(rp.SchoolInternalId))
                    {
                        if (schoolSettingsBySchool[rp.SchoolInternalId].ContainsKey("Grades/PreReportDays"))
                        {
                            rp.DaysOpenBeforeEnd = Parsers.ParseInt(schoolSettingsBySchool[rp.SchoolInternalId]["Grades/PreReportDays"]);
                        }

                        if (schoolSettingsBySchool[rp.SchoolInternalId].ContainsKey("Grades/PostReportDays"))
                        {
                            rp.DaysOpenAfterEnd = Parsers.ParseInt(schoolSettingsBySchool[rp.SchoolInternalId]["Grades/PostReportDays"]);
                        }
                    }
                }
                return returnMe;
            }
        }

        public ReportPeriod Get(int reportPeriodID)
        {
            ReportPeriod returnMe = null;

            using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
            {
                // We'll need a list of the school's settings to see when the report periods open and close
                // Organize this list by school
                Dictionary<int, Dictionary<string, string>> schoolSettingsBySchool = new Dictionary<int, Dictionary<string, string>>();
                SchoolSettingsRepository settingsrepository = new SchoolSettingsRepository();
                foreach (SchoolSetting setting in settingsrepository.GetAll())
                {
                    if (!schoolSettingsBySchool.ContainsKey(setting.SchoolDatabaseID))
                    {
                        schoolSettingsBySchool.Add(setting.SchoolDatabaseID, new Dictionary<string, string>());
                    }
                    schoolSettingsBySchool[setting.SchoolDatabaseID].Add(setting.Key, setting.Value);
                }

                // Now we can load the actual report periods
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT ReportPeriod.iReportPeriodID, ReportPeriod.dStartDate, ReportPeriod.dEndDate, ReportPeriod.iTermID, ReportPeriod.cName, ReportPeriod.mComment, ReportPeriod.lPostMarks, ReportPeriod.iSchoolID, School.cCode FROM ReportPeriod LEFT OUTER JOIN School ON ReportPeriod.iSchoolID = School.iSchoolID WHERE ReportPeriod.iReportPeriodID=@REPORTPERIODID;"
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


                // Add additional information from the school's settings
                if (schoolSettingsBySchool.ContainsKey(returnMe.SchoolInternalId))
                {
                    if (schoolSettingsBySchool[returnMe.SchoolInternalId].ContainsKey("Grades/PreReportDays"))
                    {
                        returnMe.DaysOpenBeforeEnd = Parsers.ParseInt(schoolSettingsBySchool[returnMe.SchoolInternalId]["Grades/PreReportDays"]);
                    }

                    if (schoolSettingsBySchool[returnMe.SchoolInternalId].ContainsKey("Grades/PostReportDays"))
                    {
                        returnMe.DaysOpenAfterEnd = Parsers.ParseInt(schoolSettingsBySchool[returnMe.SchoolInternalId]["Grades/PostReportDays"]);
                    }
                }
                
                return returnMe;
            }
        }
    }
}