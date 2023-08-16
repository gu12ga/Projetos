const app = require('./app');

require('dotenv').config();

PORT = process.env.PORT || 3335;

app.listen(PORT, ()=> console.log('Server funcionando'));