using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SchoolLogicAPI.Models;

namespace SchoolLogicAPI.Repositories
{
    public class SchoolRepository
    {
        private School SQLDataReaderToSchool(SqlDataReader dataReader)
        {
            return new School()
            {
                ID = Parsers.ParseInt(dataReader["iSchoolID"].ToString().Trim()),
                Address = dataReader["mAddress"].ToString().Trim(),
                AlternateCode = dataReader["cAlternateCode"].ToString().Trim(),
                Code = dataReader["cCode"].ToString().Trim(),
                DistrictID = Parsers.ParseInt(dataReader["iDistrictID"].ToString().Trim()),
                HighGrade = dataReader["cHighGrade"].ToString().Trim(),
                LowGrade = dataReader["cLowGrade"].ToString().Trim(),
                Name = dataReader["cName"].ToString().Trim()
            };
        }

        public List<School> GetAll()
        {
            List<School> returnMe = new List<School>();

            using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT School.iSchoolID, School.iDistrictID, School.cName, School.cCode, School.cAlternateCode, School.iLV_TypeID, School.cPicturePath, School.mRegistrationKey, School.mAddress, School.cPrincipal, School.lInactive, School.cCLC, School.iLV_RegionID, School.cTLogicKey, School.iCountryID, School.iLocationID, Grades.cName AS cLowGrade, Grades_1.cName AS cHighGrade FROM School LEFT OUTER JOIN Grades ON School.iLow_GradesID = Grades.iGradesID LEFT OUTER JOIN Grades AS Grades_1 ON School.iHigh_GradesID = Grades_1.iGradesID;"
                };
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        School loadedSchool = SQLDataReaderToSchool(dbDataReader);
                        if (loadedSchool != null)
                        {
                            returnMe.Add(loadedSchool);
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }

            return returnMe;
        }

        public List<School> GetByDistrict(int districtID)
        {
            List<School> returnMe = new List<School>();

            using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT School.iSchoolID, School.iDistrictID, School.cName, School.cCode, School.cAlternateCode, School.iLV_TypeID, School.cPicturePath, School.mRegistrationKey, School.mAddress, School.cPrincipal, School.lInactive, School.cCLC, School.iLV_RegionID, School.cTLogicKey, School.iCountryID, School.iLocationID, Grades.cName AS cLowGrade, Grades_1.cName AS cHighGrade FROM School LEFT OUTER JOIN Grades ON School.iLow_GradesID = Grades.iGradesID LEFT OUTER JOIN Grades AS Grades_1 ON School.iHigh_GradesID = Grades_1.iGradesID WHERE iDistrictID=@DISTRICTID;"
                };
                sqlCommand.Parameters.AddWithValue("DISTRICTID", districtID);
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        School loadedSchool = SQLDataReaderToSchool(dbDataReader);
                        if (loadedSchool != null)
                        {
                            returnMe.Add(loadedSchool);
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }

            return returnMe;
        }
        public School Get(int schoolID)
        {
            School returnMe = new School();

            using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT School.iSchoolID, School.iDistrictID, School.cName, School.cCode, School.cAlternateCode, School.iLV_TypeID, School.cPicturePath, School.mRegistrationKey, School.mAddress, School.cPrincipal, School.lInactive, School.cCLC, School.iLV_RegionID, School.cTLogicKey, School.iCountryID, School.iLocationID, Grades.cName AS cLowGrade, Grades_1.cName AS cHighGrade FROM School LEFT OUTER JOIN Grades ON School.iLow_GradesID = Grades.iGradesID LEFT OUTER JOIN Grades AS Grades_1 ON School.iHigh_GradesID = Grades_1.iGradesID WHERE School.iSchoolID=@SCHOOLID;"
                };
                sqlCommand.Parameters.AddWithValue("SCHOOLID", schoolID);
                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        School loadedSchool = SQLDataReaderToSchool(dbDataReader);
                        if (loadedSchool != null)
                        {
                            returnMe = loadedSchool;
                        }
                    }
                }

                sqlCommand.Connection.Close();
            }

            return returnMe;
        } 

    }
}