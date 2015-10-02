using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SchoolLogicAPI.Models;

namespace SchoolLogicAPI.Repositories
{
    public class TermRepository
    {
        private Term SQLDataReaderToTerm(SqlDataReader dataReader)
        {
            return new Term()
            {
                ID = Parsers.ParseInt(dataReader["iTermID"].ToString().Trim()),
                TrackID = Parsers.ParseInt(dataReader["iTrackID"].ToString().Trim()),
                StartDate = Parsers.ParseDate(dataReader["dStartDate"].ToString().Trim()),
                EndDate = Parsers.ParseDate(dataReader["dEndDate"].ToString().Trim()),
                Name = dataReader["cName"].ToString().Trim(),
                SchoolID = Parsers.ParseInt(dataReader["iSchoolID"].ToString().Trim())
            };
        }

        public Term Get(int id)
        {
            Term returnMe = new Term();

            using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM Term WHERE iTermID=@TERMID;"
                };
                sqlCommand.Parameters.AddWithValue("TERMID", id);
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        Term loadedObject = SQLDataReaderToTerm(dbDataReader);
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

        public List<Term> GetAll()
        {
            List<Term> returnMe = new List<Term>();

            using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM Term;"
                };
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        Term loadedObject = SQLDataReaderToTerm(dbDataReader);
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


    }
}