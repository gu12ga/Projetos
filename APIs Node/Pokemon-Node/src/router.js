const express = require('express');
const controller = require('./Controllers/controller.js');

const router = express.Router();

router.get('/', (request, response)=> response.status(200).send('Hello world'));
router.get('/teste', controller.get);
router.get('/teste2', controller.getAll);
router.post('/upload', controller.uploadFile);

module.exports = router;