const express = require('express');
const router = require('./router');

const app = express();

app.use(express.json());
app.use(router);

app.listen(3333, ()=> console.log('Server funcionando'));

module.exports = app;