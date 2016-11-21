using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.IO;

/// <summary>
/// the primary interface for sql read/writes. most of our database relevant code goes here.
/// 
/// :::::::::::::::::::::::::Matt TODO::::::::::::::::::::::
/// GenerateNewTiles()
/// DropPickupRewards()
/// FillwithProps()
/// SummonEnemies()
/// AquirePowerup()
/// GetSaves()
/// CreateNewSave();
/// </summary>
public class SQLInterface : MonoBehaviour {

    IDbConnection dbconn;
    public int currentActiveSavefile;
    /// <summary>
    /// Built in Unity function used for initialization.
    /// this function is called a single time, upon activation.
    /// though this is not a constructor, it is similar enough
    /// that you should be doing constructor-y stuff here.
    /// </summary>
    void Start () {
        currentActiveSavefile = 0;// 0 is a nonexistent save
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
        "CREATE TABLE contains_character ( "+
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
        ");";
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
            "insert into enemy values(1,'SlimeBig',4,5);"+
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
            "insert into pickup values(8,'FlameSword',50,1); ";
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
    //Functions that need doing
    ////////////////////////////////////////////////////////

    /// <summary>
    /// gets as string representation the save entries of the database. 
    /// output int is index of save.
    /// output string is name of save.
    /// </summary>
    /// <returns> a list of KV pairs(int, string)</returns>
    public List<KeyValuePair<int,String>> GetSaves()
    {
        return null;
    }
    /// <summary>
    /// generate a new entry for save_files.
    /// leave sf_id null, sql will autogenerate.
    /// random generate a seed somehow, use MathF or something.
    /// This must also generate new entries for Player_Character, 
    /// contains_character,map_tiles(at coord 0,0), and contains_tile.
    /// </summary>
    /// <param name="name">The name of the save file</param>
    public void CreateNewSave(String name)
    {
        return;
    }

    /// <summary>
    /// Identifies maptiles that should be adjacent to the maptile specified by nextcoord,
    /// but do not yet exist in the database. adds new entries to map_tiles and contains_tiles
    /// as needed,and returns the indexes of the newly instanced map tiles.
    /// </summary>
    /// <param name="prevtile">The tile the character just left. ignore from search.</param>
    /// <param name="nextile">The tile the character is currently leaving.</param>
    /// <returns> the indexes of all newly instanced tiles.</returns>
    public List<int> GenerateNewTiles(int prevtile, int nexttile)
    {
        return null;
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
    /// </summary>
    /// <param name="pickup">index of pickup to add</param>
    /// <param name="playercharacter">index of character to add pickup to</param>
    public void AquirePowerup(int pickup, int playercharacter)
    {
        return;
    }


}
