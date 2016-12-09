using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.IO;
[RequireComponent(typeof(GameManager))]
/// <summary>
/// the primary interface for sql read/writes. most of our database relevant code goes here.
/// 
/// :::::::::::::::::::::::::Matt TODO::::::::::::::::::::::
/// like everything marked down below past line 250
/// </summary>
public class SQLInterface : MonoBehaviour {
    IDbConnection dbconn;
    /// <summary>
    /// Built in Unity function used for initialization.
    /// this function is called a single time, upon activation.
    /// though this is not a constructor, it is similar enough
    /// that you should be doing constructor-y stuff here.
    /// </summary>
    void Start () {
        //instantiate a new database if one does not exist.
        String filePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)+ "\\TopdownGame\\topdown.db";
        if (File.Exists(filePath))
        {
            Debug.Log("<color=green>"+filePath + " found.</color> connecting to database.");
            ConnectToExistingDB(filePath);
        }
        else
        {
            Debug.Log("<color=green>"+filePath + " was not found.</color>  proceeding to instance database.");
            InstantiateFreshDB(filePath);
            PopulateStaticTables();
        }

        //CreateNewPropInstances( 1, 2);
        }
    /// <summary>
    /// Built in unity function.
    /// called upon game close.
    /// </summary>
    void OnApplicationQuit()
    {
        Disconnect();
    }
	/// <summary>
    /// built in unity function.
    /// called once every frame.
    /// </summary>
	void Update () {
	
	}

    /// <summary>
    /// disconnect from the database.
    /// </summary>
    public void Disconnect() {
        if (dbconn != null)
        {
            Debug.Log("Disposing of db connection.");
            dbconn.Dispose();
        }
    }
    /// <summary>
    /// Connect to a database without generating tables.
    /// </summary>
    /// <param name="filepath"> the path the to the database file</param>
    public void ConnectToExistingDB(String filepath)
    {
        Debug.Log("Initializing db connection.");
        string conn = "URI=file:"+filepath; //Path to database.
        dbconn = new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.


    }

    /// <summary>
    /// generates a new, empty database.
    /// </summary>
    public void InstantiateFreshDB(String filepath)
    {
        (new FileInfo(filepath)).Directory.Create();
        string conn = "URI=file:" + filepath; //Path to database.
        dbconn = new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        //gigantic schema constructor 
        Debug.Log("Constructing empty db.");
        dbcmd.CommandText=
            "BEGIN;" +
        "CREATE TABLE contains_character ( " +
        "cc_saveID INTEGER NOT NULL "+
        "REFERENCES save_files (sf_id), "+
        "cc_characterID DECIMAL(15) NOT NULL "+
        "REFERENCES player_character(pc_ID) "+
        ");"+
        "CREATE TABLE contains_enemy( " +
        "ce_tileID INTEGER NOT NULL " +
        "REFERENCES map_tiles(t_id), " +
        "ce_enemyID INTEGER NOT NULL " +
        "REFERENCES enemy(e_typeID), " +
        "ce_lLocX DECIMAL(5, 0) NOT NULL, " +
        "ce_lLocY DECIMAL(5, 0) NOT NULL, " +
        "ce_instancedhp DECIMAL(10, 0) NOT NULL " +
        ");"+
        "CREATE TABLE contains_pickup( " +
        "cp_tileID INTEGER NOT NULL " +
        "REFERENCES map_tiles(t_id), " +
        "cp_pickupID INTEGER NOT NULL " +
        "REFERENCES pickup(p_typeID), " +
        "cp_lLocX DECIMAL(5, 0) NOT NULL, " +
        "cp_lLocY DECIMAL(5, 0) NOT NULL " +
        "); "+
        "CREATE TABLE contains_prop( " +
        "cpr_tileID INTEGER NOT NULL " +
        "REFERENCES map_tiles(t_id), " +
        "cpr_propID INTEGER NOT NULL " +
        "REFERENCES prop(pr_typeID), " +
        "cpr_lLocX DECIMAL(5, 0) NOT NULL, " +
        "cpr_lLocY DECIMAL(5, 0) NOT NULL " +
        ");"+
        "CREATE TABLE contains_tile( " +
        "ct_saveID INTEGER NOT NULL " +
        "REFERENCES save_files(sf_id), " +
        "ct_tileID INTEGER NOT NULL " +
        "REFERENCES map_tiles(t_id) " +
        ");"+
        "CREATE TABLE enemy( " +
        "e_typeID INTEGER NOT NULL " +
        "PRIMARY KEY, " +
        "e_name CHAR(25) NOT NULL, " +
        "e_exp DECIMAL(10) NOT NULL, " +
        "e_maxHealth DECIMAL(10) NOT NULL " +
        ");"+
        "CREATE TABLE has_powerups( " +
        "hp_characterID INTEGER NOT NULL " +
        "REFERENCES player_character(pc_ID), " +
        "hp_pickupID INTEGER NOT NULL " +
        "REFERENCES pickup(p_typeID) ," +
        "hp_count INTEGER NOT NULL "+
        ");"+
        "CREATE TABLE map_tiles( " +
        "t_id INTEGER NOT NULL " +
        "PRIMARY KEY, " +
        "t_gcoord_x INTEGER NOT NULL, " +
        "t_gcoord_y INTEGER NOT NULL, " +
        "t_save_id INTEGER NOT NULL " +
        "REFERENCES save_files(sf_id), " +
        "t_spawnrate TIME NOT NULL, " +
        "t_last_visited TIME DEFAULT(0) " +
        "NOT NULL, " +
        "t_max_pickups INTEGER NOT NULL, " +
        "t_max_enemies INTEGER NOT NULL " +
        "DEFAULT(0) " +
        ");"+
        "CREATE TABLE pickup( " +
        "p_typeID INTEGER NOT NULL " +
        "PRIMARY KEY, " +
        "p_name CHAR(25) NOT NULL, " +
        "p_exp DECIMAL(10) NOT NULL, " +
        "p_isActive BOOLEAN NOT NULL " +
        ");"+
        "CREATE TABLE player_character( " +
        "pc_ID INTEGER NOT NULL " +
        "PRIMARY KEY, " +
        "pc_name CHAR(25) NOT NULL, " +
        "pc_gCoordX INTEGER NOT NULL, " +
        "pc_gCoordY INTEGER NOT NULL, " +
        "pc_lCoordX DECIMAL(5, 0) NOT NULL, " +
        "pc_lCoordY DECIMAL(5, 0) NOT NULL, " +
        "pc_exp DECIMAL(10) NOT NULL, " +
        "pc_health DECIMAL(10) NOT NULL, " +
        "pc_damage DECIMAL(10) NOT NULL " +
        ");"+
        "CREATE TABLE prop( " +
        "pr_typeID INTEGER NOT NULL " +
        "PRIMARY KEY, " +
        "pr_name CHAR(25) NOT NULL, " +
        "pr_size_x DECIMAL(15, 0) NOT NULL, " +
        "pr_size_y DECIMAL(15, 0) NOT NULL " +
        ");" +
        "CREATE TABLE save_files( " +
        "sf_id INTEGER NOT NULL " +
        "PRIMARY KEY, " +
        "sf_name CHAR(25) NOT NULL, " +
        "sf_seed DECIMAL(10) NOT NULL, " +
        "sf_date_created DATETIME NOT NULL " +
        ");"+
        "CREATE TABLE highscore ( "+
        "hs_id INTEGER NOT NULL "+
        "PRIMARY KEY,"+
        "hs_name CHAR(25) NOT NULL, "+
        "hs_exp  DECIMAL(10) NOT NULL "+
        ");"+
        "COMMIT;";

        dbcmd.ExecuteNonQuery();
        Debug.Log("Empty database constructed.");
        dbcmd.Dispose();
    }

    /// <summary>
    /// Fill the Enemy, Prop, and Pickup Tables with data.
    /// </summary>
    public void PopulateStaticTables()
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        Debug.Log("Populating Static db tables.");
        dbcmd.CommandText =
            "BEGIN;" +
            "insert into enemy values(1,'SlimeBig',4,5);" +
            "insert into enemy values(2,'SlimeMed',3,3);" +
            "insert into enemy values(3,'SlimeSmall',2,1);" +
            "insert into enemy values(4,'Skeltal',4,8); " +
            "insert into enemy values(5,'SkeltalArcher',4,3); " +
            "insert into enemy values(6,'SkeltalShield',6,4); " +
            "insert into enemy values(7,'WizardGuy',8,4); " +
            "insert into enemy values(8,'Bat',2,2); " +

            "insert into prop values(1,'Barrel',3,3); " +
            "insert into prop values(2,'Table',4,12); " +
            "insert into prop values(3,'WallHoriz',30,4); " +
            "insert into prop values(4,'WallVert',4,30); " +
            "insert into prop values(5,'WallBendLBot',30,30); " +
            "insert into prop values(6,'WallBendRTop',30,30); " +
            "insert into prop values(7,'SpikedPole',4,4); " +
            "insert into prop values(8,'Chandelier',8,8); " +

            "insert into pickup values(1,'HealthSmall',5,1); " +
            "insert into pickup values(2,'HealthBig',5,1); " +
            "insert into pickup values(3,'TreasureChest',20,1); " +
            "insert into pickup values(4,'Coin',10,1); " +
            "insert into pickup values(5,'Arrow',5,1); " +
            "insert into pickup values(6,'Shield',50,1); " +
            "insert into pickup values(7,'Bow',50,1); " +
            "insert into pickup values(8,'FlameSword',50,1); " +
            "COMMIT;";

        dbcmd.ExecuteNonQuery();
        dbcmd.Dispose();
        Debug.Log("Populated static tables.");

    }
    /// <summary>
    /// syntax for sqlite queries.
    /// </summary>
    public void ExampleQuery()
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = 
            "SELECT * "+ 
            "FROM Map_Tiles";
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            Debug.Log(reader.GetString(0));
        }
        reader.Close();
        dbcmd.Dispose();

    }

    ////////////////////////////////////////////////////////
    //Functions that need doing by matt
    ////////////////////////////////////////////////////////

    /// <summary>
    /// gets saves as string representations the save entries of the database. 
    /// output int is index of save.
    /// output string is name of save.
    /// </summary>
    /// <returns> a list of KV pairs(int, string)</returns>
    public List<KeyValuePair<int,String>> GetSaves()
    {
        List<KeyValuePair<int, string>> list = new List<KeyValuePair<int,string>>();
        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = 
            "SELECT sf_id, sf_name "+ 
            "FROM save_files";
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int index = reader.GetInt32(reader.GetOrdinal("sf_id"));
            string name = reader.GetString(reader.GetOrdinal("sf_name"));
            Debug.Log("Loading Save "+ reader.GetInt32(0));
            list.Add(new KeyValuePair<int,string>(index, name));

        }
        reader.Close();
        dbcmd.Dispose();


        return list;
    }
    

    /// <summary>
    /// pull the data of the character associated with a save file. 
    /// 
    /// </summary>
    /// <param name="saveIndex"> the index of the save</param>
    /// <returns></returns>
    public CharacterStats getAssociatedCharacterStats(int saveIndex)
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = 
            "SELECT pc_id, pc_name, pc_exp, pc_damage, pc_lCoordX, pc_lCoordY, pc_gCoordX, pc_gCoordY "+ 
            "FROM player_character,contains_character,save_files "+
            "WHERE pc_id = cc_characterID " +
            "AND cc_saveID = sf_id; ";
        IDataReader reader = dbcmd.ExecuteReader();
        int index = 0;
        string name = "meow";
        float exp = 0;
        float dmg = 0;
        Vector2 local_coord = Vector2.zero ;
        IntVector global_coord = new IntVector();
        while (reader.Read())
        {
            index = reader.GetInt32(reader.GetOrdinal("pc_id"));
            name = reader.GetString(reader.GetOrdinal("pc_name"));
            exp = reader.GetFloat(reader.GetOrdinal("pc_exp"));
            dmg = reader.GetFloat(reader.GetOrdinal("pc_damage"));
            local_coord = new Vector2(reader.GetFloat(reader.GetOrdinal("pc_lCoordX")), reader.GetFloat(reader.GetOrdinal("pc_lCoordY")));
            global_coord = new IntVector( reader.GetInt32(reader.GetOrdinal("pc_gCoordX")), reader.GetInt32(reader.GetOrdinal("pc_gCoordY")));


        }
        CharacterStats stats = new CharacterStats(index, name, global_coord, local_coord, exp, dmg);
        reader.Close();
        dbcmd.Dispose();
        return stats;
    }
    /// <summary>
    /// get all tiles associated with a save file.
    /// int is tile index, Intvector is its global coord
    /// </summary>
    /// <param name="saveIndex"> the index of the save</param>
    /// <returns></returns>
    public List<KeyValuePair<int, IntVector>> getAssociatedTiles(int saveIndex)
    {
        List<KeyValuePair<int, IntVector>> list = new List<KeyValuePair<int, IntVector>>();

        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText =
            "SELECT t_id, t_gcoord_x, t_gcoord_y " +
            "FROM map_tiles " +
            "WHERE t_save_id =  " + saveIndex + " ;";
        IDataReader reader = dbcmd.ExecuteReader();
        int tileindex = 0;
        IntVector global_coord = new IntVector();
        while (reader.Read())
        {
            tileindex = reader.GetInt32(reader.GetOrdinal("t_id"));
            global_coord = new IntVector(reader.GetInt32(reader.GetOrdinal("t_gcoord_x")), reader.GetInt32(reader.GetOrdinal("t_gcoord_y")));
            list.Add(new KeyValuePair<int, IntVector>(tileindex, global_coord));

        }
        reader.Close();
        dbcmd.Dispose();
        return list;
    }
    /// <summary>
    /// pull the data of the has_enmemy associated with a level tile
    /// </summary>
    /// <param name="tileIndex"></param>
    /// <returns></returns>
    public List<EnemyStats> getAssociatedEnemies(int tileIndex)
    {
        List<EnemyStats> list = new List<EnemyStats>();

        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText =
            "SELECT e_typeID, e_name, e_exp, e_maxHealth, ce_lLocX, ce_lLocY, ce_instancehp, t_gcoord_x, t_gcoord_y " +
            "FROM enemy, contains_enemy, map_tile " +
            "WHERE " + tileIndex + " = ce_tileID " +
            "AND ce_enemyID = e_ID; ";
        IDataReader reader = dbcmd.ExecuteReader();
        int type = 0;
        string name = "wabuffet";
        float exp = 0;
        float maxhp = 0;
        IntVector global_coords = new IntVector();
        Vector2 local_coords = Vector2.zero;
        float currenthp = 0;
        while (reader.Read())
        {
            type = reader.GetInt32(reader.GetOrdinal("e_typeID"));
            name = reader.GetString(reader.GetOrdinal("e_name"));
            exp = reader.GetFloat(reader.GetOrdinal("e_exp"));
            maxhp = reader.GetFloat(reader.GetOrdinal("e_maxHealth"));
            currenthp = reader.GetFloat(reader.GetOrdinal("ce_instancehp"));
            global_coords = new IntVector(reader.GetInt32(reader.GetOrdinal("t_gcoord_x")),reader.GetInt32(reader.GetOrdinal("t_gcoord_y")));
            local_coords = new Vector2(reader.GetFloat(reader.GetOrdinal("ce_lLocX")), reader.GetFloat(reader.GetOrdinal("ce_lLocY")));
            Debug.Log(reader.GetString(0));
            list.Add(new EnemyStats(type, name, global_coords, local_coords, exp, maxhp, currenthp));

        }

        reader.Close();
        dbcmd.Dispose();
        return list;
    }
    /// <summary>
    /// pull the has_prop data associated with a level tile
    /// </summary>
    /// <param name="tileindex"></param>
    /// <returns></returns>
    public List<PropStats> getAssociatedProps(int tileindex)
    {
        List<PropStats> list = new List<PropStats>();

        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText =
            "SELECT pr_typeID, pr_name, pr_size_x, pr_size_y, cpr_lLocX, cpr_lLocY, t_gcoord_x, t_gcoord_y " +
            "FROM prop, contains_prop, map_tiles " +
            "WHERE " + tileindex + " = cpr_tileID " +
            "AND cpr_propID = pr_typeID; ";
        IDataReader reader = dbcmd.ExecuteReader();
        int type = 0;
        string name = "wabuffet";
        Vector2 size = Vector2.zero;
        IntVector global_coords = new IntVector();
        Vector2 local_coords = Vector2.zero;
        while (reader.Read())
        {
            type = reader.GetInt32(reader.GetOrdinal("pr_typeID"));
            name = reader.GetString(reader.GetOrdinal("pr_name"));
            global_coords = new IntVector(reader.GetInt32(reader.GetOrdinal("t_gcoord_x")), reader.GetInt32(reader.GetOrdinal("t_gcoord_y")));
            local_coords = new Vector2(reader.GetFloat(reader.GetOrdinal("cpr_lLocX")), reader.GetFloat(reader.GetOrdinal("cpr_lLocY")));
            size = new Vector2(reader.GetFloat(reader.GetOrdinal("pr_size_x")), reader.GetFloat(reader.GetOrdinal("pr_size_y")));
            list.Add(new PropStats(type, name, global_coords, local_coords, size));

            
         }
        return list;
    }
    public List<PickupStats> getAssociatedPickups(int tileindex)
    {
        List<PickupStats> list = new List<PickupStats>();

        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText =
            "SELECT p_typeID, p_name, p_exp, p_isActive, cp_lLocX, cp_lLocY, t_gcoord_x, t_gcoord_y " +
            "FROM pickup, contains_pickup, map_tile " +
            "WHERE " + tileindex + " = cp_tileID " +
            "AND cp_pickupID = p_typeID; ";
        IDataReader reader = dbcmd.ExecuteReader();
        int type = 0;
        string name = "wabuffet";
        float exp = 0;
        ///bool active = false;
        IntVector global_coords = new IntVector();
        Vector2 local_coords = Vector2.zero;
        while (reader.Read())
        {
            type = reader.GetInt32(reader.GetOrdinal("p_typeID"));
            name = reader.GetString(reader.GetOrdinal("p_name"));
            global_coords = new IntVector(reader.GetInt32(reader.GetOrdinal("t_gcoord_x")), reader.GetInt32(reader.GetOrdinal("t_gcoord_y")));
            local_coords = new Vector2(reader.GetFloat(reader.GetOrdinal("cp_lLocX")), reader.GetFloat(reader.GetOrdinal("cp_lLocY")));
            exp = reader.GetFloat(reader.GetOrdinal("p_exp"));
            Debug.Log(reader.GetString(0));
            list.Add(new PickupStats(type, name, global_coords, local_coords, exp));


        }
        return list;
    }

    public List<KeyValuePair<int, Vector2>> CreateNewPropInstances(int tileindex,int count)
    {
        List<KeyValuePair<int,Vector2 >> retlist = new List<KeyValuePair<int, Vector2>>();
        IDbCommand dbcmd = dbconn.CreateCommand();

        string buildstatement = "BEGIN;";
        for (int i = 0; i < count; i++)
        {
            int proptype = (int)UnityEngine.Random.Range(1, 9);
            Vector2 locoords = new Vector2(UnityEngine.Random.Range(-40f, 40f), UnityEngine.Random.Range(-40f, 40f));
            buildstatement += "INSERT INTO contains_prop values( " + tileindex + " , " +proptype +" , "+ locoords.x+" , " + locoords.y+ "); ";
            retlist.Add(new KeyValuePair<int, Vector2>(proptype, locoords));
        }
        buildstatement += " COMMIT;";
        dbcmd.CommandText = buildstatement;
        dbcmd.ExecuteNonQuery();
        dbcmd.Dispose();
        return retlist;
    }

    /// <summary>
    /// generate a new entry for save_files.
    /// leave sf_id null, sql will autogenerate.
    /// random generate a seed somehow, use MathF or something.
    /// This must also generate new entries for Player_Character, 
    /// contains_character,map_tiles(at coord 0,0), and contains_tile.
    /// map tile should have a maximum enemies of 0, and a maximum pickups of 0.
    /// returns index of newly generated save.
    /// </summary>
    /// <param name="name">The name of the save file</param>
    public int CreateNewSave(String Savename, String Playername)
    {
        string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string seed = String.Format("#{0:X8}", UnityEngine.Random.Range(0, int.MaxValue));

        // get newest index for save file
        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = "SELECT IFNULL( MAX(sf_id) , 0) AS 'index' FROM save_files";
        int newSaveIndex = 0;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            newSaveIndex = reader.GetInt32(reader.GetOrdinal("index"))+1;

        }
        dbcmd.Dispose();

        // get newest index for player
        dbcmd.CommandText = "SELECT IFNULL( MAX(pc_ID), 0 ) as 'index' FROM player_character";
        reader = dbcmd.ExecuteReader();
        int newPlayerIndex = 0;
        while (reader.Read())
        {

            newPlayerIndex = reader.GetInt32(reader.GetOrdinal("index"))+1;
        }
        dbcmd.Dispose();

        dbcmd.CommandText = "SELECT IFNULL(MAX(t_id),0) AS 'index' FROM map_tiles";
        reader = dbcmd.ExecuteReader();
        int newMapIndex = 0;
        while (reader.Read())
        {

            newMapIndex = reader.GetInt32(reader.GetOrdinal("index")) + 1;
        }
        dbcmd.Dispose();


        dbcmd.CommandText =
            "INSERT INTO save_files VALUES ( " + newSaveIndex + " , '" + Savename + "' , '" + seed + "' , '" + time + "' ); "+
            "INSERT INTO player_character VALUES( "+ newPlayerIndex+" , '" + Playername + "' , " + 0 + " , " + 0 + " , " + 0 + " , " + 0 + " , " + 0 + " , " + 12 + " , " + 1 + " ); " +
            "INSERT INTO map_Tiles VALUES( "+ newMapIndex+", 0, 0, "+newSaveIndex+ ", 0, '" +time+ "' , 0 , 0  );"+
            "INSERT INTO map_Tiles VALUES( " + (newMapIndex+1) + ", 1, 0, "   + newSaveIndex + ", 1, '" + "0000-01-01 00:00:00" + "' , 2 , 3  );" +
            "INSERT INTO map_Tiles VALUES( " + (newMapIndex+2) + ", 0, 1, "   + newSaveIndex + ", 1, '" + "0000-01-01 00:00:00" + "' , 2 , 3  );" +
            "INSERT INTO map_Tiles VALUES( " + (newMapIndex+3) + ", 1, 1, "   + newSaveIndex + ", 1, '" + "0000-01-01 00:00:00" + "' , 2 , 3  );" +
            "INSERT INTO map_Tiles VALUES( " + (newMapIndex+4) + ", -1, 0, "  + newSaveIndex + ", 1, '" + "0000-01-01 00:00:00" + "' , 2 , 3  );" +
            "INSERT INTO map_Tiles VALUES( " + (newMapIndex+5) + ", 0, -1, "  + newSaveIndex + ", 1, '" + "0000-01-01 00:00:00" + "' , 2 , 3  );" +
            "INSERT INTO map_Tiles VALUES( " + (newMapIndex+6) + ", -1, -1, " + newSaveIndex + ", 1, '" + "0000-01-01 00:00:00" + "' , 2 , 3  );" +
            "INSERT INTO map_Tiles VALUES( " + (newMapIndex+7) + ", 1, -1, "  + newSaveIndex + ", 1, '" + "0000-01-01 00:00:00" + "' , 2 , 3  );" +
            "INSERT INTO map_Tiles VALUES( " + (newMapIndex+8) + ", -1, 1, "  + newSaveIndex + ", 1, '" + "0000-01-01 00:00:00" + "' , 2 , 3  );" +
            "INSERT INTO contains_tile VALUES( " + newSaveIndex + " , " + newMapIndex + " );" +
            "INSERT INTO contains_tile VALUES( " + newSaveIndex + " , " + (newMapIndex +1) + " );" +
            "INSERT INTO contains_tile VALUES( " + newSaveIndex + " , " + (newMapIndex +2) + " );" +
            "INSERT INTO contains_tile VALUES( " + newSaveIndex + " , " + (newMapIndex +3) + " );" +
            "INSERT INTO contains_tile VALUES( " + newSaveIndex + " , " + (newMapIndex +4) + " );" +
            "INSERT INTO contains_tile VALUES( " + newSaveIndex + " , " + (newMapIndex +5) + " );" +
            "INSERT INTO contains_tile VALUES( " + newSaveIndex + " , " + (newMapIndex +6) + " );" +
            "INSERT INTO contains_tile VALUES( " + newSaveIndex + " , " + (newMapIndex +7) + " );" +
            "INSERT INTO contains_tile VALUES( " + newSaveIndex + " , " + (newMapIndex +8) + " );" +
            "INSERT INTO contains_character VALUES( " + newSaveIndex+" , "+ newPlayerIndex+ " );"
            ;
        dbcmd.ExecuteNonQuery();
        Debug.Log("Checkpoint");
        dbcmd.Dispose();
        return newSaveIndex;
    }

    /// <summary>
    /// Identifies maptiles that should be adjacent to the maptile specified by nextcoord,
    /// but do not yet exist in the database. adds new entries to map_tiles and contains_tiles
    /// as needed,and returns the indexes of the newly instanced map tiles.
    /// </summary>
    /// <param name="prevtile">The tile the character just left. ignore from search.</param>
    /// <param name="nextile">The tile the character is currently entering.</param>
    /// <returns> the indexes of all newly instanced tiles.</returns>
    public List<KeyValuePair<int,IntVector>> GenerateNewTiles(IntVector prevtile, IntVector nexttile, int saveIndex)
    {

        ///Not sure how to approach >  insert a new tile based on t_id of current tile? > so an insert into map_tiles Values( nexttile +
        ///gcoord_x +index_x{this to calculate where the new tile will be as you add a value onto the current coord based on the direction taken(not sure how thats determined)} and same thing for y coord,
        ///then pair this with the contains file and save files tables(by updating those 2?)
        ///this one seems to be mostly sql side, except for the needed indexes
        ///
        List<KeyValuePair<int, IntVector>> newtiles= new List<KeyValuePair<int, IntVector>>();


        // generate positions for new tiles
        IntVector check1 = nexttile+nexttile - prevtile;
        IntVector check2= check1;
        IntVector check3= check1;
        if (nexttile.x-prevtile.x != 0)
        { // then the three to check are in a vert line

            check2.y += 1;
            check3.y -= 1;

        }
        else
        {
            check2.x += 1;
            check3.x -= 1;
        }
        //Debug.Log("Checking for existence of tiles at " + check1.x + " , " + check1.y);
        //Debug.Log("Checking for existence of tiles at " + check2.x + " , " + check2.y);
        //Debug.Log("Checking for existence of tiles at " + check3.x + " , " + check3.y);

        // get fresh map tile index
        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = "SELECT IFNULL( MAX(t_id), 0 ) as 'index' FROM map_tiles";
        int newindex = Convert.ToInt32(dbcmd.ExecuteScalar())+1;
        int numberofpickups = UnityEngine.Random.Range(0, 2);
        int numberofenemies = UnityEngine.Random.Range(2, 6);
        //int numberofpickups = 2;
        //int numberofenemies = 2;
        // start doing checks for exists 
        dbcmd.CommandText = "select 1 from map_tiles where t_gcoord_x= " + check1.x + " and t_gcoord_y = " + check1.y +" and t_save_id = "+saveIndex+ ";";
        var scalar1instance=dbcmd.ExecuteScalar();
        string buildtext = "BEGIN;  ";
        if (scalar1instance==null)
        {
            buildtext += "INSERT INTO map_Tiles VALUES( " + newindex + ", "+ check1.x+" , "+ check1.y +" , " + saveIndex + ", 1, '" 
                            + "0000-01-01 00:00:00" + "' , "+ numberofpickups + " , "+ numberofenemies + " );" +
                             "INSERT INTO contains_tile VALUES( " + saveIndex + " , " + newindex + " );";
            newtiles.Add(new KeyValuePair<int, IntVector>(newindex, check1));
            newindex++;

            
        }

        dbcmd.CommandText = "select 1 from map_tiles where t_gcoord_x= " + check2.x + " and t_gcoord_y = " + check2.y + " and t_save_id = " + saveIndex + ";";

        var scalar2instance = dbcmd.ExecuteScalar();

        if (scalar2instance==null)
        {
            buildtext += "INSERT INTO map_Tiles VALUES( " + newindex + ", " + check2.x + " , " + check2.y + " , " + saveIndex + ", 1, '"
                                        + "0000-01-01 00:00:00" + "' , " + numberofpickups + " , " + numberofenemies + " );" +
                                         "INSERT INTO contains_tile VALUES( " + saveIndex + " , " + newindex + " ); ";
            newtiles.Add(new KeyValuePair<int, IntVector>(newindex, check2));
            newindex++;
        }

        dbcmd.CommandText = "select 1 from map_tiles where t_gcoord_x= " + check3.x + " and t_gcoord_y = " + check3.y + " and t_save_id = " + saveIndex + ";";
        var scalar3instance = dbcmd.ExecuteScalar();

        if (scalar3instance==null)
        {

            buildtext += "INSERT INTO map_Tiles VALUES( " + newindex + ", " + check3.x + " , " + check3.y + " , " + saveIndex + ", 1, '"
                                        + "0000-01-01 00:00:00" + "' , " + numberofpickups + " , " + numberofenemies + " );" +
                                         "INSERT INTO contains_tile VALUES( " + saveIndex + " , " + newindex + " ); ";
            newtiles.Add(new KeyValuePair<int, IntVector>(newindex, check3));

            newindex++;
        }
        dbcmd.CommandText = buildtext+" COMMIT; ";
        dbcmd.ExecuteNonQuery();
        dbcmd.Dispose();

        return newtiles;
    }
    /// <summary>
    /// Generate up to the max number of contains_pickups possible for a specified tile. 
    /// for pickups 6, 7, and 8 (shield, flamingsword, and bow), if the character's has_powerups already
    /// contains these items, do not generate entries with those pickup ids.
    /// local x and local y must be in the range (-40,40). 
    /// 
    /// </summary>
    /// <param name="currenttile"> the index of the tile to generate on.</param>
    /// <returns> the indexes of all newly instanced contains_pickups. </returns>
    public List<int> DropPickupRewards(int currenttile, int playerCharacter)
    {
        ///theres no max to consider as it isnt in the db?
        return null;
    }
    /// <summary>
    /// generate up to the max number of contains_enemies possible for a specified tile.
    /// you should not exceed the tiles max_ enemies, if there are already enemies in a 
    /// tile, only generate as many entries as needed. 
    /// </summary>
    /// <param name="currenttile"></param>
    public void SummonEnemies(int currenttile)
    {
        ///mostly sql
        ///no idea how to populate a tile with enemies
        ///I understand the constraints, but how to you populate a table which has different columns from another table with the count of 
        ///enemies(essentially) as well as having to determine which enemy to spawn as well
        ///mainly becuase theres nothing that tracks the current number of enemies within said tile
        return;
    }
    /// <summary>
    /// generate up to the max number of contains_props possile for a specified tile.
    /// if a tile has any contains_props at all, do not generate any new entries.
    /// props cannot collide with the outer boundaries of a tile: abs(prop size.x+prop local coords.x)
    /// cannot exceed 40, as tiles are 100x100 units in size, and there needs to be a gap for the player
    /// to walk through.
    /// </summary>
    /// <param name="currenttile">the index of the tile to generate on.</param>
    /// <returns> the indexes of all newly instanced contains_props. </returns>
    public List<int> FillwithProps(int currenttile)
    {
        return null;
    }
    /// <summary>
    /// adds a pickup to has_powerups. If an entry already exists, increment hp_count.
    /// if an entry does not yet exist, create a new entry. 
    /// remove the contains_pickups from the database.
    /// 
    /// if the pickup is a healthkit, call HealPlayer().
    /// if the pickup is a chest, call ActivateChest().
    /// if the pickup is an arrow, instead increment hp_count by 10( arrow pickups grant 10 arrows), and call ActivateArrows()
    /// if the pickup is a bow, call ActivateBow().
    /// if the pickup is a shield, call ActivateShield().
    /// if the pickup is a flamesword, call ActivateFlameSword().
    /// 
    /// </summary>
    /// <param name="contains_pickup">index of instantiated pickup to add</param>
    /// <param name="playercharacter">index of character to add pickup to</param>
    public void AquirePowerup(int contains_pickup, int playercharacter)
    {
        return;
    }
    /// <summary>
    /// restore health to player character, up to 12 hp.
    /// if bigheals, restore 4 hp. otherwise,restore 2.
    /// </summary>
    /// <param name="player"> index of player character to modify health.</param>
    /// <param name="bigheals">whether the health kit is a big or small</param>
    public void healPlayer(int player, bool bigheals)
    {
        //SQL STUFF HERE.
        return;
    }

    /// <summary>
    /// remove this chest from the database, and generate three new random contains_pickup entries, 
    /// where the local locations are +- 10 units away from the original chest. again, dont instance
    /// flameswords, shields, or bows if the player's has_powerups already has one.
    /// </summary>
    /// <param name="chest"></param>
    /// <returns> the indexes of the newly created entries</returns>
    public List<int> ActivateChest(int player, int chest)
    {
        return null;
    }

    ////////////////////////////////////////////////////////
    //End of Matt's stuff
    ////////////////////////////////////////////////////////



    public void ActivateArrows(int player)
    {
    }
    public void ActivateBow() { }
    public void ActivateShield() { }
    public void ActivateFlameSword() { }
}
