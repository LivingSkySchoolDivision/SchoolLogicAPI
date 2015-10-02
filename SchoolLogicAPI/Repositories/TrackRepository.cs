using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SchoolLogicAPI.Models;

namespace SchoolLogicAPI.Repositories
{
    public class TrackRepository
    {
        private Track SQLDataReaderToTrack(SqlDataReader dataReader)
        {
            return new Track()
            {
                ID = Parsers.ParseInt(dataReader["iTrackID"].ToString().Trim()),
                Name = dataReader["cName"].ToString().Trim(),
                Code = dataReader["cCode"].ToString().Trim(),
                StartDate = Parsers.ParseDate(dataReader["dStartDate"].ToString().Trim()),
                EndDate = Parsers.ParseDate(dataReader["dEndDate"].ToString().Trim()),
                IsAttendanceDaily = Parsers.ParseBool(dataReader["lDaily"].ToString().Trim()),
                SchoolID = Parsers.ParseInt(dataReader["iSchoolID"].ToString().Trim()),
                DaysInCycle = Parsers.ParseInt(dataReader["iDaysInCycle"].ToString().Trim()),
                BlocksPerDay = Parsers.ParseInt(dataReader["iBlocksPerDay"].ToString().Trim()),
                DailyBlocksPerDay = Parsers.ParseInt(dataReader["iDailyBlocksPerDay"].ToString().Trim()),
                EffortLegendID = Parsers.ParseInt(dataReader["iEffortLegendID"].ToString().Trim())
            };
        }

        public List<Track> GetAll()
        {
            List<Track> returnMe = new List<Track>();

            using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM Track;"
                };
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        Track loadedObject = SQLDataReaderToTrack(dbDataReader);
                        if (loadedObject != null)
                        {
                            returnMe.Add(loadedObject);
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }

            return returnMe;
        }


        public Track Get(int trackID)
        {
            Track returnMe = new Track();

            using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM Track WHERE iTrackID=@TRACKID;"
                };
                sqlCommand.Parameters.AddWithValue("TRACKID", trackID);
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        Track loadedObject = SQLDataReaderToTrack(dbDataReader);
                        if (loadedObject != null)
                        {
                            returnMe = loadedObject;
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }

            return returnMe;
        } 
    }
}