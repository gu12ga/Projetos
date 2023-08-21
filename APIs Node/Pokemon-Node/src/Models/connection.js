const mysql = require('mysql2/promise');

require('dotenv').config();

const connection = mysql.createPool({
  host: '*******',
  user: '*******',
  password: '*******',
  database: '*******',
});

module.exports = connection;