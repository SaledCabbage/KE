using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
//
using System.Data;
using System.Data.SQLite;
using System.Data.Common;
namespace Ke.Utils
{
    public class DB
    {

        public SQLiteConnection _connection;
        private string DB_PATH;
        public DB()
        {
            this.DB_PATH = "data\\ETTS_test.db";
            this.DB_PATH = AppDomain.CurrentDomain.BaseDirectory + DB_PATH;
        }
        public DB(string db_path)
        {
            this.DB_PATH = db_path;
        }
        /// <summary>
        /// SQLite连接
        /// </summary>
        public SQLiteConnection connection
        {
            get
            {
                if (_connection == null)
                {
                    //string DB_PATH = "data\\course.db";
                    //string pt = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                    SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
                    connstr.DataSource = DB_PATH;
                    //connstr.Password = "";
                    string sttt = connstr.ToString();
                    _connection = new SQLiteConnection();
                    _connection.ConnectionString = connstr.ToString();
                    _connection = new SQLiteConnection(connstr.ToString());
                    _connection.Open();
                }
                return _connection;
            }
        }
        

        /// <summary>
        /// SQLite增删改
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="parameters">所需参数</param>
        /// <returns>所受影响的行数</returns>
        public int ExecuteNonQuery(string sql, SQLiteParameter[] parameters)
        {
            int affectedRows = 0;

            DbTransaction transaction = connection.BeginTransaction();
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = sql;
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            affectedRows = command.ExecuteNonQuery();
            transaction.Commit();

            return affectedRows;
        }

        /// <summary>
        /// SQLite查询
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="parameters">所需参数</param>
        /// <returns>结果DataTable</returns>
        public DataTable ExecuteDataTable(string sql, SQLiteParameter[] parameters)
        {
            DataTable data = new DataTable();

            SQLiteCommand command = new SQLiteCommand(sql, connection);
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            int num = adapter.Fill(data);
            DataView dv = data.DefaultView;

            return data;
        }

        /// <summary>
        /// 查询数据库表信息
        /// </summary>
        /// <returns>数据库表信息DataTable</returns>
        public DataTable GetSchema()
        {
            DataTable data = new DataTable();

            data = connection.GetSchema("TABLES");

            return data;
        }
        //学生操作
        public Student getStudentById(int id)
        {
            Student student = new Student();
            SQLiteCommand cmd = this.connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM USER WHERE _id = " + id;
            System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {

                try { student.id = reader.GetInt16(0); }
                catch (Exception e) { }
                try { student.USERNAME = reader.GetString(1); }
                catch (Exception e) { }
                try { student.password = reader.GetString(2); }
                catch (Exception e) { }
                try { student.NAME = reader.GetString(3); }
                catch (Exception e) { }
                try { student.age = reader.GetInt16(4); }
                catch (Exception e) { }
                try { student.sex = reader.GetString(5); }
                catch (Exception e) { }
                try { student.phone = reader.GetString(6); }
                catch (Exception e) { }
                try { student.email = reader.GetString(7); }
                catch (Exception e) { }
                try { student.address = reader.GetString(8); }
                catch (Exception e) { }
                try { student.remark = reader.GetString(9); }
                catch (Exception e) { }
                try { student.isAdmin = reader.GetInt16(11); }
                catch (Exception e) { }
                try
                {

                    MemoryStream streamImage = new MemoryStream(reader["PHOTO"] as byte[]);
                    byte[] desBytes = new byte[streamImage.Length];
                    streamImage.Read(desBytes, 0, desBytes.Length);
                    streamImage.Close();
                    student.photo = desBytes;
                    streamImage.Close(); // 关闭流
                }
                catch (Exception e) { }
            }
            else
            {
                return null;
            }
            reader.Close();
            return student;
        }

        public Student getStudentByName(string name)
        {
            Student student = new Student();
            SQLiteCommand cmd = this.connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM USER WHERE USERNAME = \"" + name + "\"";
            System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {

                try { student.id = reader.GetInt16(0); }
                catch (Exception e) { }
                try { student.USERNAME = reader.GetString(1); }
                catch (Exception e) { }
                try { student.password = reader.GetString(2); }
                catch (Exception e) { }
                try { student.NAME = reader.GetString(3); }
                catch (Exception e) { }
                try { student.age = reader.GetInt16(4); }
                catch (Exception e) { }
                try { student.sex = reader.GetString(5); }
                catch (Exception e) { }
                try { student.phone = reader.GetString(6); }
                catch (Exception e) { }
                try { student.email = reader.GetString(7); }
                catch (Exception e) { }
                try { student.address = reader.GetString(8); }
                catch (Exception e) { }
                try { student.remark = reader.GetString(9); }
                catch (Exception e) { }
                try { student.isAdmin = reader.GetInt16(11); }
                catch (Exception e) { }
                try
                {

                    MemoryStream streamImage = new MemoryStream(reader["PHOTO"] as byte[]);
                    byte[] desBytes = new byte[streamImage.Length];
                    streamImage.Read(desBytes, 0, desBytes.Length);
                    streamImage.Close();
                    student.photo = desBytes;
                    streamImage.Close(); // 关闭流
                }
                catch (Exception e) { }
            }
            else
            {
                return null;
            }
            reader.Close();
            return student;
        }

        public int insertStudent(Student student)
        {
            if (student == null)
                return 0;

            SQLiteParameter[] parameters = new SQLiteParameter[11];
            string sql = "INSERT INTO USER(USERNAME, PASSWORD, NAME, AGE, SEX, PHONE, EMAIL, ADDRESS, REMARK, PHOTO, IS_ADMIN) VALUES(@mUSERNAME, @mPASSWORD, @mNAME, @mAGE, @mSEX, @mPHONE, @mEMAIL, @mADDRESS, @mREMARK, @mPHOTO, @mIS_ADMIN)";
            //parameters[0]=(new SQLiteParameter("m_id", student.id));
            parameters[0] = (new SQLiteParameter("mUSERNAME", student.USERNAME));
            parameters[1] = (new SQLiteParameter("mPASSWORD", student.password));
            parameters[2] = (new SQLiteParameter("mNAME", student.NAME));
            parameters[3] = (new SQLiteParameter("mAGE", student.age));
            parameters[4] = (new SQLiteParameter("mSEX", student.sex));
            parameters[5] = (new SQLiteParameter("mPHONE", student.phone));
            parameters[6] = (new SQLiteParameter("mEMAIL", student.email));
            parameters[7] = (new SQLiteParameter("mADDRESS", student.address));
            parameters[8] = (new SQLiteParameter("mREMARK", student.remark));

            SQLiteParameter p = new SQLiteParameter("mPHOTO", DbType.Binary);
            p.Value = student.photo;
            parameters[9] = p;
            parameters[10] = (new SQLiteParameter("mIS_ADMIN", student.isAdmin));
            return this.ExecuteNonQuery(sql, parameters);

        }
        public int updateStudent(Student student)
        {
            if (student == null)
                return 0;

            SQLiteParameter[] parameters = new SQLiteParameter[12];
            //string sql = "UPDATE USER SET(USERNAME, PASSWORD, NAME, AGE, SEX, PHONE, EMAIL, ADDRESS, REMARK, PHOTO, IS_ADMIN) VALUES(@mUSERNAME, @mPASSWORD, @mNAME, @mAGE, @mSEX, @mPHONE, @mEMAIL, @mADDRESS, @mREMARK, @mPHOTO, @mIS_ADMIN) WHERE _id=" + student.id;
            string sql = "UPDATE USER SET USERNAME = @mUSERNAME, PASSWORD = @mPASSWORD, NAME = @mNAME, AGE = @mAGE, SEX = @mSEX, PHONE = @mPHONE, EMAIL = @mEMAIL, ADDRESS = @mADDRESS , REMARK = @mREMARK, PHOTO = @mPHOTO, IS_ADMIN = @mIS_ADMIN WHERE _id = @m_id";

            //VALUES(@mUSERNAME, @mPASSWORD, @mNAME, @mAGE, @mSEX, @mPHONE, @mEMAIL, @mADDRESS, @mREMARK, @mPHOTO, @mIS_ADMIN) WHERE _id=" + student.id;
            parameters[11] = (new SQLiteParameter("m_id", student.id));
            parameters[0] = (new SQLiteParameter("mUSERNAME", student.USERNAME));
            parameters[1] = (new SQLiteParameter("mPASSWORD", student.password));
            parameters[2] = (new SQLiteParameter("mNAME", student.NAME));
            parameters[3] = (new SQLiteParameter("mAGE", student.age));
            parameters[4] = (new SQLiteParameter("mSEX", student.sex));
            parameters[5] = (new SQLiteParameter("mPHONE", student.phone));
            parameters[6] = (new SQLiteParameter("mEMAIL", student.email));
            parameters[7] = (new SQLiteParameter("mADDRESS", student.address));
            parameters[8] = (new SQLiteParameter("mREMARK", student.remark));
            //parameters[8] = (new SQLiteParameter("mPHOTO", student.photo));

            SQLiteParameter p = new SQLiteParameter("mPHOTO", DbType.Binary);
            p.Value = student.photo;
            parameters[9] = p;
            parameters[10] = (new SQLiteParameter("mIS_ADMIN", student.isAdmin));
            return this.ExecuteNonQuery(sql, parameters);

        }
        // 老师操作
        public Teacher getTeacherById(int id)
        {
            Teacher teacher = new Teacher();
            SQLiteCommand cmd = this.connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM TEACHER WHERE _id = " + id;
            System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {

                try { teacher._id = reader.GetInt16(0); }
                catch (Exception e) { }
                try { teacher.TEACHERNUMBER = reader.GetString(1); }
                catch (Exception e) { }
                try { teacher.NAME = reader.GetString(2); }
                catch (Exception e) { }
                try { teacher.AGE = reader.GetInt16(3); }
                catch (Exception e) { }
                try { teacher.SEX = reader.GetString(4); }
                catch (Exception e) { }
                try { teacher.PHONE = reader.GetString(5); }
                catch (Exception e) { }
                try { teacher.EMAIL = reader.GetString(6); }
                catch (Exception e) { }
                try { teacher.POSITION = reader.GetString(7); }
                catch (Exception e) { }
                try { teacher.DIRECTION = reader.GetString(8); }
                catch (Exception e) { }
                try { teacher.DETAILS = reader.GetString(9); }
                catch (Exception e) { }
                try { teacher.REMARK = reader.GetString(10); }
                catch (Exception e) { }
                try
                {
                    MemoryStream streamImage = new MemoryStream(reader["PHOTO"] as byte[]);
                    byte[] desBytes = new byte[streamImage.Length];
                    streamImage.Read(desBytes, 0, desBytes.Length);
                    streamImage.Close();
                    teacher.PHOTO = desBytes;
                    streamImage.Close(); // 关闭流
                }
                catch (Exception e) { }
            }
            else
            {
                return null;
            }
            reader.Close();
            return teacher;
        }
        public int insertTeacher(Teacher teacher)
        {
            if (teacher == null)
                return 0;

            SQLiteParameter[] parameters = new SQLiteParameter[11];
            string sql = "INSERT INTO TEACHER(TEACHERNUMBER, NAME, AGE, SEX, PHONE, EMAIL, POSITION, DIRECTION, DETAILS, REMARK, PHOTO) VALUES(@mTEACHERNUMBER, @mNAME, @mAGE, @mSEX, @mPHONE, @mEMAIL, @mPOSITION, @mDIRECTION, @mDETAILS, @mREMARK, @mPHOTO)";
            //parameters[0]=(new SQLiteParameter("m_id", student.id));
            parameters[0] = (new SQLiteParameter("mTEACHERNUMBER", teacher.TEACHERNUMBER));
            parameters[1] = (new SQLiteParameter("mNAME", teacher.NAME));
            parameters[2] = (new SQLiteParameter("mAGE", teacher.AGE));
            parameters[3] = (new SQLiteParameter("mSEX", teacher.SEX));
            parameters[4] = (new SQLiteParameter("mPHONE", teacher.PHONE));
            parameters[5] = (new SQLiteParameter("mEMAIL", teacher.EMAIL));
            parameters[6] = (new SQLiteParameter("mPOSITION", teacher.POSITION));
            parameters[7] = (new SQLiteParameter("mDIRECTION", teacher.DIRECTION));
            parameters[8] = (new SQLiteParameter("mDETAILS", teacher.DETAILS));
            parameters[9] = (new SQLiteParameter("mREMARK", teacher.REMARK));

            SQLiteParameter p = new SQLiteParameter("mPHOTO", DbType.Binary);
            p.Value = teacher.PHOTO;
            parameters[10] = p;
            return this.ExecuteNonQuery(sql, parameters);
        }

        public int updateTeacher(Teacher teacher)
        {
            if (teacher == null)
                return 0;

            SQLiteParameter[] parameters = new SQLiteParameter[12];
            string sql = "UPDATE TEACHER SET TEACHERNUMBER = @mTEACHERNUMBER, NAME = @mNAME,  SEX = @mSEX, PHONE = @mPHONE,  REMARK = @mREMARK, AGE = @mAGE,POSITION = @mPOSITION, EMAIL = @mEMAIL , DETAILS = @mDETAILS, PHOTO = @mPHOTO, DIRECTION = @mDIRECTION WHERE _id = @m_id";

            parameters[11] = (new SQLiteParameter("m_id", teacher._id));
            parameters[0] = (new SQLiteParameter("mTEACHERNUMBER", teacher.TEACHERNUMBER));
            parameters[1] = (new SQLiteParameter("mNAME", teacher.NAME));
            parameters[2] = (new SQLiteParameter("mSEX", teacher.SEX));
            parameters[3] = (new SQLiteParameter("mAGE", teacher.AGE));
            parameters[4] = (new SQLiteParameter("mPHONE", teacher.PHONE));
            parameters[5] = (new SQLiteParameter("mEMAIL", teacher.EMAIL));
            parameters[6] = (new SQLiteParameter("mPOSITION", teacher.POSITION));
            parameters[7] = (new SQLiteParameter("mDIRECTION", teacher.DIRECTION));
            parameters[8] = (new SQLiteParameter("mDETAILS", teacher.DETAILS));
            parameters[9] = (new SQLiteParameter("mREMARK", teacher.REMARK));

            SQLiteParameter p = new SQLiteParameter("mPHOTO", DbType.Binary);
            p.Value = teacher.PHOTO;
            parameters[10] = p;
            return this.ExecuteNonQuery(sql, parameters);
        }
        //课程操作
        public Course getCourseById(int id)
        {
            Course course = new Course();
            SQLiteCommand cmd = this.connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM COURSE WHERE _id = " + id;
            System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                try { course._id = reader.GetInt16(0); }
                catch (Exception e) { }
                try { course.COURSENUMBER = reader.GetString(1); }
                catch (Exception e) { }
                try { course.NAME = reader.GetString(2); }
                catch (Exception e) { }
                try { course.CATEGORY = reader.GetString(3); }
                catch (Exception e) { }
                try { course.TEACHER = reader.GetString(4); }
                catch (Exception e) { }
                try { course.TIME = reader.GetString(5); }
                catch (Exception e) { }
                try { course.PRICE = reader.GetFloat(6); }
                catch (Exception e) { }
                try { course.DETAILS = reader.GetString(7); }
                catch (Exception e) { }
                try { course.REMARK = reader.GetString(8); }
                catch (Exception e) { }
                try
                {//course.PHOTO = reader.GetString(9);



                    MemoryStream streamImage = new MemoryStream(reader["PHOTO"] as byte[]);
                    byte[] desBytes = new byte[streamImage.Length];
                    streamImage.Read(desBytes, 0, desBytes.Length);
                    streamImage.Close();
                    course.PHOTO = desBytes;
                    streamImage.Close(); // 关闭流
                }
                catch (Exception e) { }
            }
            reader.Close();
            return course;
        }
        public int insertCourse(Course course)
        {
            if (course == null)
                return 0;

            SQLiteParameter[] parameters = new SQLiteParameter[9];
            string sql = "INSERT INTO COURSE(COURSENUMBER, NAME, CATEGORY, TEACHER, TIME, PRICE, DETAILS, REMARK,PHOTO) VALUES(@mCOURSENUMBER, @mNAME, @mCATEGORY, @mTEACHER, @mTIME, @mPRICE, @mDETAILS, @mREMARK,@mPHOTO)";
            //parameters[0]=(new SQLiteParameter("m_id", student.id));
            parameters[0] = (new SQLiteParameter("@mCOURSENUMBER", course.COURSENUMBER));
            parameters[1] = (new SQLiteParameter("@mNAME", course.NAME));
            parameters[2] = (new SQLiteParameter("@mCATEGORY", course.CATEGORY));
            parameters[3] = (new SQLiteParameter("@mTEACHER", course.TEACHER));
            parameters[4] = (new SQLiteParameter("@mTIME", course.TIME));
            parameters[5] = (new SQLiteParameter("@mPRICE", course.PRICE));
            parameters[6] = (new SQLiteParameter("@mDETAILS", course.DETAILS));
            parameters[7] = (new SQLiteParameter("@mREMARK", course.REMARK));
            //parameters[9] = (new SQLiteParameter("mREMARK", course.PHOTO));

            SQLiteParameter p = new SQLiteParameter("mPHOTO", DbType.Binary);
            p.Value = course.PHOTO;
            parameters[8] = p;
            return this.ExecuteNonQuery(sql, parameters);
        }
        public int updateCourse(Course course)
        {
            if (course == null)
                return 0;

            SQLiteParameter[] parameters = new SQLiteParameter[10];
            string sql = "UPDATE COURSE SET COURSENUMBER = @mCOURSENUMBER, NAME = @mNAME,  CATEGORY = @mCATEGORY, TEACHER = @mTEACHER,  TIME = @mTIME, PRICE = @mPRICE, DETAILS = @mDETAILS , REMARK = @mREMARK, PHOTO = @mPHOTO WHERE _id = @m_id";
            parameters[0] = (new SQLiteParameter("m_id", course._id));
            parameters[1] = (new SQLiteParameter("@mCOURSENUMBER", course.COURSENUMBER));
            parameters[2] = (new SQLiteParameter("@mNAME", course.NAME));
            parameters[3] = (new SQLiteParameter("@mCATEGORY", course.CATEGORY));
            parameters[4] = (new SQLiteParameter("@mTEACHER", course.TEACHER));
            parameters[5] = (new SQLiteParameter("@mTIME", course.TIME));
            parameters[6] = (new SQLiteParameter("@mPRICE", course.PRICE));
            parameters[7] = (new SQLiteParameter("@mDETAILS", course.DETAILS));
            parameters[8] = (new SQLiteParameter("@mREMARK", course.REMARK));
            //parameters[9] = (new SQLiteParameter("mREMARK", course.PHOTO));

            SQLiteParameter p = new SQLiteParameter("mPHOTO", DbType.Binary);
            p.Value = course.PHOTO;
            parameters[9] = p;
            return this.ExecuteNonQuery(sql, parameters);
        }
        //课表操作
        public Schedule getScheduleById(int id)
        {
            Schedule schedule = new Schedule();
            SQLiteCommand cmd = this.connection.CreateCommand();
            //SELECT ORDERS.ORDERNUMBER as 订单号, COURSE.NAME as 课程名, COURSE.COURSENUMBER as 课程编号, USER.NAME as 学生姓名, USER._id as 学生编号 from ORDERS inner join USER on ORDERS.[USER_ID] = USER.[_id] inner join COURSE on ORDERS.COURSE_ID = COURSE._id 
            cmd.CommandText = "SELECT * FROM ORDERS WHERE STATUS = 0 AND _id = " + id;
            System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                schedule._id = reader.GetInt16(0);
                schedule.ORDERNUMBER = reader.GetString(1);
                schedule.COURSE_ID = reader.GetInt16(2);
                schedule.USER_ID = reader.GetInt16(3);
                schedule.ADDTIME = reader.GetInt64(4);
                schedule.time = this.GetTime(schedule.ADDTIME + "").ToString();
                schedule.userName = this.getStudentById(schedule.USER_ID).NAME;
                schedule.courseName = this.getCourseById(schedule.COURSE_ID).NAME;
                schedule.REMARK = reader.GetString(8);
                //schedule.time = 
                //course.PHOTO = reader.GetString(9);



            }
            reader.Close();
            return schedule;
        }
        public int insertSchedule(Schedule schedule)
        {
            if (schedule == null)
                return 0;

            SQLiteParameter[] parameters = new SQLiteParameter[11];
            string sql = "insert into ORDERS(ORDERNUMBER,COURSE_ID,USER_ID,ADDTIME,STATUS,DEV_ID) VALUES(@mORDERNUMBER,@mCOURSE_ID,@mUSER_ID,@mADDTIME,@mSTATUS,DEV_ID)";
            //parameters[0]=(new SQLiteParameter("m_id", student.id));
            parameters[0] = (new SQLiteParameter("@mORDERNUMBER", schedule.ORDERNUMBER));
            parameters[1] = (new SQLiteParameter("@mORDERNUMBER", schedule.ORDERNUMBER));
            parameters[2] = (new SQLiteParameter("@mUSER_ID", schedule.USER_ID));
            parameters[4] = (new SQLiteParameter("@mADDTIME", schedule.ADDTIME));
            parameters[4] = (new SQLiteParameter("@mSTATUS", schedule.STATUS));
            parameters[4] = (new SQLiteParameter("@mDEV_ID", schedule.DEV_ID));
            return this.ExecuteNonQuery(sql, parameters);
        }

        /**
        *添加订单状态修改的功能
        */
        public int updateORDER(Schedule Schedule)
        {
            if (Schedule == null)
                return 0;

            SQLiteParameter[] parameters = new SQLiteParameter[2];
            string sql = "UPDATE ORDERS SET REMARK = @mREMARK WHERE _id = @m_id";
            parameters[0] = (new SQLiteParameter("m_id", Schedule._id));
            parameters[1] = (new SQLiteParameter("@mREMARK", Schedule.REMARK));

            return this.ExecuteNonQuery(sql, parameters);
        }

        public DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            //DateTime dtStart = new DateTime(1970, 1, 1);
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime); 
            return dtStart.Add(toNow);
        }
        public int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        public int Delete(string table, string where)
        {
            //DELETE FROM table_name  WHERE[condition];

            string sql = "DELETE FROM " + table + " WHERE " + where;
            return this.ExecuteNonQuery(sql, null);

        }
        public int Delete_(string table, string where)
        {
            //DELETE FROM table_name  WHERE[condition];

            string sql = "UPDATE " + table + " SET STATUS = 1 WHERE " + where;
            return this.ExecuteNonQuery(sql, null);

        }
        /**
         * 验证登录。
         * 3 无此用户
         * 2 密码不对
         * 1 非管理员
         * 0 成功
         */
        public int LogIn(string name, string password)
        {
            Student user = this.getStudentByName(name);
            if (user == null)
            {
                return 3;
            }
            else if (!user.password.Equals(password))
            {
                return 2;
            }
            else if (user.isAdmin == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }
        public int merge_ORDER(DB db2)
        {
            int sum = 0;
            int change=0;
            SQLiteCommand cmd2 = new SQLiteCommand("select * from ORDERS",db2.connection);
            SQLiteDataReader reader2 = cmd2.ExecuteReader();

            SQLiteCommand cmd1 = this.connection.CreateCommand();
   
            SQLiteDataReader reader1 = null;
            if(reader2.HasRows)
            { 
                while(reader2.Read())
                {
                    long s1 = reader2.GetInt64(2);
                    string s2 = reader2.GetString(1);

                    cmd1.CommandText = "select * from ORDERS where ORDERNUMBER=\"" + reader2.GetString(1) + "\" and COURSE_ID = " + reader2.GetInt64(2);
                    reader1 = cmd1.ExecuteReader();
                    if (!reader1.HasRows)
                    {
                        SQLiteCommand cmdTemp = new SQLiteCommand(connection);
                        cmdTemp.CommandText = "insert into ORDERS(ORDERNUMBER,COURSE_ID,USER_ID,ADDTIME,STATUS,DEV_ID,EXPECTED_TIME,REMARK) VALUES(@mORDERNUMBER,@mCOURSE_ID,@mUSER_ID,@mADDTIME,@mSTATUS,@mDEV_ID,@mEXPECTED_TIME,@mREMARK)";

                        SQLiteParameter[] parameters = new SQLiteParameter[8];
                        parameters[0] = (new SQLiteParameter("@mORDERNUMBER", reader2.GetString(1)));
                        parameters[1] = (new SQLiteParameter("@mCOURSE_ID", reader2.GetInt64(2)));
                        parameters[2] = (new SQLiteParameter("@mUSER_ID", reader2.GetInt64(3)));
                        parameters[3] = (new SQLiteParameter("@mADDTIME", reader2.GetInt64(4)));
                        parameters[4] = (new SQLiteParameter("@mSTATUS", reader2.GetInt64(5)));
                        parameters[5] = (new SQLiteParameter("@mDEV_ID", reader2.GetString(6)));
                        parameters[6] = (new SQLiteParameter("@mEXPECTED_TIME", reader2.GetString(7)));
                        parameters[7] = (new SQLiteParameter("@mREMARK", reader2.GetString(8)));

                        cmdTemp.Parameters.AddRange(parameters);
                        int affected = cmdTemp.ExecuteNonQuery();
                        if (affected > 0)
                            sum++;
                    }
                    else//update
                    {
                        SQLiteCommand cmdTemp = new SQLiteCommand(connection);
                        //cmdTemp.CommandText = "insert into ORDERS(ORDERNUMBER,COURSE_ID,USER_ID,ADDTIME,STATUS,DEV_ID,EXPECTED_TIME,REMARK) VALUES(@mORDERNUMBER,@mCOURSE_ID,@mUSER_ID,@mADDTIME,@mSTATUS,@mDEV_ID,@mEXPECTED_TIME,@mREMARK)";
                        cmdTemp.CommandText = "update ORDERS set ORDERNUMBER=@mORDERNUMBER,COURSE_ID=@mCOURSE_ID,USER_ID=@mUSER_ID,ADDTIME=@mADDTIME,STATUS=@mSTATUS,DEV_ID=@mDEV_ID,EXPECTED_TIME=@mEXPECTED_TIME,REMARK=@mREMARK where ORDERNUMBER=\"" + reader2.GetString(1) + "\" and COURSE_ID = " + reader2.GetInt64(2);
                        
                        SQLiteParameter[] parameters = new SQLiteParameter[8];
                        parameters[0] = (new SQLiteParameter("@mORDERNUMBER", reader2.GetString(1)));
                        parameters[1] = (new SQLiteParameter("@mCOURSE_ID", reader2.GetInt64(2)));
                        parameters[2] = (new SQLiteParameter("@mUSER_ID", reader2.GetInt64(3)));
                        parameters[3] = (new SQLiteParameter("@mADDTIME", reader2.GetInt64(4)));
                        parameters[4] = (new SQLiteParameter("@mSTATUS", reader2.GetInt64(5)));
                        parameters[5] = (new SQLiteParameter("@mDEV_ID", reader2.GetString(6)));
                        parameters[6] = (new SQLiteParameter("@mEXPECTED_TIME", reader2.GetString(7)));
                        parameters[7] = (new SQLiteParameter("@mREMARK", reader2.GetString(8)));
                        

                        cmdTemp.Parameters.AddRange(parameters);
                        int affected = cmdTemp.ExecuteNonQuery();
                        if (affected > 0)
                            sum++;
                    }
                    reader1.Close();
                }
            }
           


            return sum;
        }

    }
}
