using System;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using Newtonsoft.Json;


public class DatabaseManager
{
    #region 定义
    public static MySqlConnection mySql;

	public static string database_Name;



	/// <summary>
	/// 数据库设置的账户列列名称
	/// </summary>
	public static string database_accountID_Name = "accountID";
	/// <summary>
	/// 数据库设置的密码列列名
	/// </summary>
	public static string database_password_Name = "password";
	/// <summary>
	/// 数据库设置的数据列列名1
	/// </summary>
	public static string database_data1_Name = "data";



	#endregion

	#region 连接数据库

	/// <summary>
	/// 连接mySql数据库
	/// </summary>
	public static bool Connect(string database,string ip,int port,string user,string password)
    {
		database_Name = database;
        //创建mySql对象
        mySql = new MySqlConnection();

        //连接参数
        string s = string.Format("Database = {0} ; Data Source = {1} ; port = {2};User Id = {3} ; Passwork = {4}",database,ip,port,user,password);
        mySql.ConnectionString = s;

        try
        {
            mySql.Open();
            Console.WriteLine("[数据库]：连接成功");
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine("[数据库]：连接失败，错误：" + ex.Message);
            return false;
        }


        
    }

	#endregion

	#region 安全字符判断
	/// <summary>
	/// 安全字符判断
	/// </summary>
	private static bool IsSafeString(string str)
    {
        return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
    }

	#endregion

	#region 查询用户是否不存在
	/// <summary>
	/// 查询用户是否不存在
	/// </summary>
	public static bool IsAccountNotExist(string accountID)
	{
		//防sql注入
		if (!IsSafeString(accountID))
		{
			return false;
		}
		//sql语句
		string s = string.Format("select * from accounts where {0}='{1}';", database_accountID_Name ,accountID);

		//查询
		try
		{
			MySqlCommand mySqlCommand = new MySqlCommand(s, mySql);
			MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
			bool hasRows = mySqlDataReader.HasRows;
			mySqlDataReader.Close();
			return !hasRows;
		}
		catch (Exception ex)
		{
			Console.WriteLine("[数据库]：查询用户是否不存在时，出现错误：" + ex.Message);
			Console.WriteLine("[数据库]：请忽略下面那个报错");
			return false;
		}
	}

	#endregion

	#region 注册用户
	/// <summary>
	/// 注册用户
	/// </summary>
	public static bool Register(string accountID, string password)
	{
		//防sql注入
		if (!IsSafeString(accountID))
		{
			Console.WriteLine("[数据库]：注册失败，账户包含不安全字符！！！");
			return false;
		}
		if (!IsSafeString(password))
		{
			Console.WriteLine("[数据库]：注册失败，密码包含不安全字符！！！");
			return false;
		}
		//能否注册
		if (!IsAccountNotExist(accountID))
		{
			Console.WriteLine("[数据库]：注册失败，账户已存在！");
			return false;
		}
		//写入数据库User表
		string sql = string.Format("insert into accounts set {0} ='{1}' ,{2} ='{3}';",database_accountID_Name, accountID,database_password_Name, password);
		try
		{
			MySqlCommand mySqlCommand = new MySqlCommand(sql, mySql);
			mySqlCommand.ExecuteNonQuery();
			// Console.WriteLine("[数据库]：注册成功！");
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine("[数据库]：注册失败 " + ex.Message);
			return false;
		}
	}

	#endregion

	#region 创建角色

	/// <summary>
	/// 创建角色
	/// </summary>
	public static bool CreatePlayer(string accountID)
	{
		//防sql注入
		if (!IsSafeString(accountID))
		{
			Console.WriteLine("[数据库]：创建角色失败，账户包含不安全字符！！！");
			return false;
		}
		//序列化
		PlayerDatabase PlayerDatabase = new PlayerDatabase();
		string data = JsonConvert.SerializeObject(PlayerDatabase, new JsonSerializerSettings() {StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,NullValueHandling = NullValueHandling.Ignore});
		//写入数据库
		string sql = string.Format("insert into player set {0} ='{1}' ,{2} ='{3}';",database_accountID_Name, accountID,database_data1_Name, data);
		try
		{
			MySqlCommand mySqlCommand = new MySqlCommand(sql, mySql);
			mySqlCommand.ExecuteNonQuery();
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine("[数据库]：创建角色失败，错误： " + ex.Message);
			return false;
		}
	}

	#endregion

	#region 检测账户密码


	/// <summary>
	/// 检测账户密码
	/// </summary>
	public static bool CheckPassword(string accountID, string password)
	{
		//防sql注入
		if (!IsSafeString(accountID))
		{
			Console.WriteLine("[数据库]：检测账户密码失败，账户包含不安全字符！！！");
			return false;
		}
		if (!IsSafeString(password))
		{
			Console.WriteLine("[数据库]：检测账户密码失败，密码包含不安全字符！！！");
			return false;
		}
		//查询
		string sql = string.Format("select * from accounts where {0}='{1}' and {2}='{3}';",database_accountID_Name, accountID,database_password_Name, password);

		try
		{
			MySqlCommand mySqlCommand = new MySqlCommand(sql, mySql);
			MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
			bool hasRows = mySqlDataReader.HasRows;
			mySqlDataReader.Close();
			return hasRows;
		}
		catch (Exception ex)
		{
			Console.WriteLine("检测账户密码失败，错误：" + ex.Message);
			return false;
		}
	}

	#endregion

	#region 获取玩家数据

	/// <summary>
	/// 获取玩家数据
	/// </summary>
	public static PlayerDatabase GetPlayerDatabase(string accountID)
	{
		//防sql注入
		if (!IsSafeString(accountID))
		{
			Console.WriteLine("[数据库]：获取玩家数据失败，账户包含不安全字符！！！");
			return null;
		}

		//sql
		string sql = string.Format("select * from player where {0} ='{1}';",database_accountID_Name, accountID);
		try
		{
			//查询
			MySqlCommand mySqlCommand = new MySqlCommand(sql, mySql);
			MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
			if (!mySqlDataReader.HasRows)
			{
				mySqlDataReader.Close();
				return null;
			}
			//读取
			mySqlDataReader.Read();
			string data = mySqlDataReader.GetString(database_data1_Name);
			//反序列化
			PlayerDatabase PlayerDatabase = JsonConvert.DeserializeObject<PlayerDatabase>(data, new JsonSerializerSettings() {StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,NullValueHandling = NullValueHandling.Ignore});
			mySqlDataReader.Close();
			return PlayerDatabase;
		}
		catch (Exception ex)
		{
			Console.WriteLine("[数据库]：获取玩家数据失败，错误：" + ex.Message);
			return null;
		}
	}

	#endregion

	#region 保存更新角色

	/// <summary>
	/// 保存更新角色
	/// </summary>
	public static bool UpdatePlayerDatabase(string accountID, PlayerDatabase playerDatabase)
	{
		//防sql注入
		if (!IsSafeString(accountID))
		{
			Console.WriteLine("[数据库]：保存更新角色失败，账户包含不安全字符！！！");
			return false;
		}

		//序列化
		string data = JsonConvert.SerializeObject(playerDatabase, new JsonSerializerSettings() {StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,NullValueHandling = NullValueHandling.Ignore});
		data = MyCode.DoubleCharacter_gang(data);
		//sql
        string sql = string.Format("update player set {0}='{1}' where {2} ='{3}';",database_data1_Name, data,database_accountID_Name, accountID);
		//更新
		try
		{
			MySqlCommand mySqlCommand = new MySqlCommand(sql, mySql);
			mySqlCommand.ExecuteNonQuery();
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine("[数据库]：保存更新角色数据失败，错误： " + ex.Message);
			return false;
		}
	}

    #endregion


}







