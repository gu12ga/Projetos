const mysql = require('mysql2/promise');

require('dotenv').config();

const connection = mysql.createPool({
  host: 'localhost'/*process.env.MYSQL_HOST*/,
  user: 'gustavo'/*process.env.MYSQL_USER*/,
  password: '12345'/*process.env.MYSQL_PASSWORD*/,
  database: 'pokemons'/*process.env.MYSQL_DB*/,
});

module.exports = connection;