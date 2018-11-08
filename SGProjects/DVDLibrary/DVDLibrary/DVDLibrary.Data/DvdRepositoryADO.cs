using DVDLibrary.Models;
using DVDLibrary.Models.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDLibrary.Data
{
    public class DvdRepositoryADO : IDvdRepository
    {
        public void Delete(int dvdId)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DvdDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DvdId", dvdId);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void Edit(int dvdId, string title, int releaseYear, string directorName, string ratingName, string notes)
        {
            using(var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DvdUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                List<Director> directors = GetDirectors();
                Director director = directors.FirstOrDefault(x => x.DirectorName == directorName);

                Rating movieRating = GetRatings().SingleOrDefault(x => x.RatingName == ratingName);
                

                cn.Open();

                if (director == null)
                {
                    director = InsertNewDirector(directorName);

                }
                cmd.Parameters.AddWithValue("@DvdId", dvdId);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@ReleaseYear", releaseYear);
                cmd.Parameters.AddWithValue("@DirectorId", director.DirectorId);
                cmd.Parameters.AddWithValue("@RatingId", movieRating.RatingId);
                cmd.Parameters.AddWithValue("@Notes", notes);

                cmd.ExecuteNonQuery();
            }
        }

        private Director InsertNewDirector(string directorName)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                cn.Open();
                Director director = new Director();
                director.DirectorName = directorName;
                SqlCommand directorCmd = new SqlCommand("DirectorAdd", cn);
                directorCmd.CommandType = CommandType.StoredProcedure;


                directorCmd.Parameters.AddWithValue("@DirectorName", director.DirectorName);
                SqlParameter idParameter = new SqlParameter();
                idParameter.Direction = ParameterDirection.Output;
                idParameter.ParameterName = "@DirectorId";
                idParameter.SqlDbType = SqlDbType.Int;
                idParameter.Value = DBNull.Value;

                directorCmd.Parameters.Add(idParameter);

                using (SqlDataReader dr = directorCmd.ExecuteReader())
                {
                    director.DirectorId = int.Parse(directorCmd.Parameters["@DirectorId"].Value.ToString());
                }

                return director;
            }
        }

        public List<GetDvds> GetAllDvd()
        {
            List<GetDvds> dvds = new List<GetDvds>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        GetDvds row = new GetDvds();

                        row.DvdId = (int)dr["DvdId"];
                        row.Title = dr["Title"].ToString();
                        row.ReleaseYear = (int)dr["ReleaseYear"];
                        row.Director = dr["DirectorName"].ToString();
                        row.Rating = dr["RatingName"].ToString();
                        row.Notes = dr["Notes"].ToString();

                        dvds.Add(row);
                    }
                }
            }
            return dvds;
        }

        public List<GetDvds> GetByDirector(string directorName)
        {
            List<GetDvds> dvds = new List<GetDvds>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetByDirector", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DirectorName", directorName);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        GetDvds row = new GetDvds();

                        row.DvdId = (int)dr["DvdId"];
                        row.Title = dr["Title"].ToString();
                        row.ReleaseYear = (int)dr["ReleaseYear"];
                        row.Director = dr["DirectorName"].ToString();
                        row.Rating = dr["RatingName"].ToString();
                        row.Notes = dr["Notes"].ToString();

                        dvds.Add(row);
                    }
                }
                return dvds;
            }
        }

        public GetDvds GetById(int dvdId)
        {
            GetDvds dvd = new GetDvds();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetById", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DvdId", dvdId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        GetDvds row = new GetDvds();

                        row.DvdId = (int)dr["DvdId"];
                        row.Title = dr["Title"].ToString();
                        row.ReleaseYear = (int)dr["ReleaseYear"];
                        row.Director = dr["DirectorName"].ToString();
                        row.Rating = dr["RatingName"].ToString();
                        row.Notes = dr["Notes"].ToString();

                        return row;
                    }
                }
                return null;
            }
        }

        public List<GetDvds> GetByRating(string rating)
        {
            List<GetDvds> dvds = new List<GetDvds>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetByRating", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RatingName", rating);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        GetDvds row = new GetDvds();

                        row.DvdId = (int)dr["DvdId"];
                        row.Title = dr["Title"].ToString();
                        row.ReleaseYear = (int)dr["ReleaseYear"];
                        row.Director = dr["DirectorName"].ToString();
                        row.Rating = dr["RatingName"].ToString();
                        row.Notes = dr["Notes"].ToString();

                        dvds.Add(row);
                    }
                }
                return dvds;
            }
        }

        public List<GetDvds> GetByTitle(string title)
        {
            List<GetDvds> dvds = new List<GetDvds>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetByTitle", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Title", title);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        GetDvds row = new GetDvds();

                        row.DvdId = (int)dr["DvdId"];
                        row.Title = dr["Title"].ToString();
                        row.ReleaseYear = (int)dr["ReleaseYear"];
                        row.Director = dr["DirectorName"].ToString();
                        row.Rating = dr["RatingName"].ToString();
                        row.Notes = dr["Notes"].ToString();

                        dvds.Add(row);
                    }
                }
                return dvds;
            }
        }

        public List<GetDvds> GetByYear(int year)
        {
            List<GetDvds> dvds = new List<GetDvds>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetByYear", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ReleaseYear", year);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        GetDvds row = new GetDvds();

                        row.DvdId = (int)dr["DvdId"];
                        row.Title = dr["Title"].ToString();
                        row.ReleaseYear = (int)dr["ReleaseYear"];
                        row.Director = dr["DirectorName"].ToString();
                        row.Rating = dr["RatingName"].ToString();
                        row.Notes = dr["Notes"].ToString();

                        dvds.Add(row);
                    }
                }
                return dvds;
            }
        }

        public List<Director> GetDirectors()
        {
            List<Director> dvds = new List<Director>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetDirectors", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Director row = new Director();

                        row.DirectorId = (int)dr["DirectorId"];
                        row.DirectorName = dr["DirectorName"].ToString();

                        dvds.Add(row);
                    }
                }
            }
            return dvds;
        }

        public List<Rating> GetRatings()
        {
            List<Rating> dvds = new List<Rating>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetRatings", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Rating row = new Rating();

                        row.RatingId = (int)dr["RatingId"];
                        row.RatingName = dr["RatingName"].ToString();

                        dvds.Add(row);
                    }
                }
            }
            return dvds;
        }

        public void SaveNew(string title, int releaseYear, string directorName, string ratingName, string notes)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DvdAdd", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                List<Director> directors = GetDirectors();

                cn.Open();

                Director director = directors.FirstOrDefault(x => x.DirectorName == directorName);

                if(director == null)
                {
                    director = InsertNewDirector(directorName);
                }

                int ratingId = GetRatings().SingleOrDefault(x => x.RatingName == ratingName).RatingId;

                SqlParameter idDvdParameter = new SqlParameter();
                idDvdParameter.Direction = ParameterDirection.Output;
                idDvdParameter.ParameterName = "@DvdId";
                idDvdParameter.SqlDbType = SqlDbType.Int;
                idDvdParameter.Value = DBNull.Value;

                cmd.Parameters.Add(idDvdParameter);

                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@ReleaseYear", releaseYear);
                cmd.Parameters.AddWithValue("@DirectorId", director.DirectorId);
                cmd.Parameters.AddWithValue("@RatingId", ratingId);
                cmd.Parameters.AddWithValue("@Notes", notes);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
