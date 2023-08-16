const connection = require('./connection');

const get = async () => {
    try {
        const [pokemon_data] = await connection.execute('SELECT * FROM pokemon_data');
        console.log(pokemon_data);
        return pokemon_data;
    } catch (error) {
        console.error(error);
        throw error;
    }
};

const post = async (data) => {
    const query = 'INSERT INTO pokemon_data (Name, Pokedex_Number, Img_name, Generation, Evolution_Stage, Evolved, FamilyID, Cross_Gen, Type_1, Type_2, Weather_1, Weather_2, STAT_TOTAL, ATK, DEF, STA, Legendary, Acquireable, Spawns, Regional, Raidable, Hatchable, Shiny, Nest, New, Not_Gettable, Future_Evolve, CP_40, CP_39) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)';

  const values = [
    data['Name'],
    data['Pokedex Number'],
    data['Img name'],
    data['Generation'],
    data['Evolution Stage'],
    data['Evolved'],
    data['FamilyID'],
    data['Cross Gen'],
    data['Type 1'],
    data['Type 2'],
    data['Weather 1'],
    data['Weather 2'],
    data['STAT TOTAL'],
    data['ATK'],
    data['DEF'],
    data['STA'],
    data['Legendary'],
    data['Acquireable'],
    data['Spawns'],
    data['Regional'],
    data['Raidable'],
    data['Hatchable'],
    data['Shiny'],
    data['Nest'],
    data['New'],
    data['Not-Gettable'],
    data['Future Evolve'],
    data['100% CP @ 40'],
    data['100% CP @ 39'],
  ];
  
  for (let i = 0; i < values.length; i++) {
    if (values[i] === undefined) {
      values[i] = null;
    }
  }
    
    try {

        const [result] = await connection.execute(query, values);
        return result;

    }catch (error) {

            console.error(error);
            throw error;
    }
  };
  


module.exports = {

    get,
    post
};