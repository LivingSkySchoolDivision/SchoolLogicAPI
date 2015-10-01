using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SchoolLogicAPI.Models;

namespace SchoolLogicAPI.Repositories
{
    public class SchoolSettingsRepository
    {
        public List<SchoolSetting> GetAll()
        {
            List<SchoolSetting> returnMe = new List<SchoolSetting>();

            using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM Settings;"
                };
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        SchoolSetting loadedSetting = sqlDataReaderToSchoolSetting(dbDataReader);
                        if (loadedSetting != null)
                        {
                            returnMe.Add(loadedSetting);
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }

            return returnMe;
        }

        public List<SchoolSetting> GetForSchool(int schoolDatabaseID)
        {
            List<SchoolSetting> returnMe = new List<SchoolSetting>();

            using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM Settings WHERE iSchoolID=@SCHOOLID;"
                };
                sqlCommand.Parameters.AddWithValue("@SCHOOLID", schoolDatabaseID);
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        SchoolSetting loadedSetting = sqlDataReaderToSchoolSetting(dbDataReader);
                        if (loadedSetting != null)
                        {
                            returnMe.Add(loadedSetting);
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }

            return returnMe;
        }

        private SchoolSetting sqlDataReaderToSchoolSetting(SqlDataReader dataReader)
        {
            return new SchoolSetting()
            {
                SchoolDatabaseID = Parsers.ParseInt(dataReader["iSchoolID"].ToString().Trim()),
                Key = dataReader["cKey"].ToString().Trim(),
                Value = dataReader["cValue"].ToString().Trim(),
                DataTypeIndicator = Parsers.ParseChar(dataReader["cType"].ToString().Trim()),
                SettingID = Parsers.ParseInt(dataReader["iSettingsID"].ToString().Trim())
            };
        }

    }
}