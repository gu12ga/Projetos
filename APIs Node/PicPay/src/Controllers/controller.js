const model = require('../Models/exempleModel.js');

const get = (request, response) => {
    return response.status(200).json({ message: 'controller funciona' });
};

const getAll = async (request, response) => {
    const result = await model.get();
    return await response.status(200).json(result);
};

const payload = async (request, response) => {

  const payload = await model.postT(request.body);
  return response.status(201).json(payload);

};

const insertNewUser = async (request, response) => {

  const insert = await model.insertNewUser(request.body);
  return response.status(201).json(insert);

};

module.exports = {

  get,
  getAll,
  payload,
  insertNewUser

};