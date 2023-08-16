const express = require('express');
const controller = require('./Controllers/controller.js');

const router = express.Router();

router.get('/', (request, response)=> response.status(200).send('Hello world'));
router.get('/teste', controller.get);
router.get('/lista', controller.getAll);
router.post('/create', controller.insertNewUser);
router.post('/payload', controller.payload);

module.exports = router;