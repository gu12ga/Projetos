const model = require('../Models/exempleModel.js');
const multer = require('multer');
const path = require('path');
const xlsx = require('xlsx');

// Configuração do Multer para lidar com o upload de arquivos
const storage = multer.diskStorage({
  destination: function (req, file, cb) {
    cb(null, path.join(__dirname, '../uploads')); // Caminho absoluto para o diretório uploads
  },
  filename: function (req, file, cb) {
    const uniqueSuffix = Date.now() + '-' + Math.round(Math.random() * 1E9);
    const extname = path.extname(file.originalname);
    cb(null, file.fieldname + '-' + uniqueSuffix + extname);
  },
});

// Função para lidar com o upload de arquivos
const uploadFile = (req, res) => {
  
  const uploadMiddleware = multer({ storage }).single('excelFile');

  uploadMiddleware(req, res, async err => {
    if (err instanceof multer.MulterError) {
      return res.status(400).json({ error: 'Erro durante o upload do arquivo.' });
    } else if (err) {
      return res.status(500).json({ error: 'Erro interno do servidor.' });
    }

    if (!req.file) {
      return res.status(400).json({ error: 'Nenhum arquivo foi enviado.' });
    }

    const workbook = xlsx.readFile(req.file.path);
    const sheetName = workbook.SheetNames[0]; // Assume que o arquivo tem apenas uma planilha
    const worksheet = workbook.Sheets[sheetName];
    const data = xlsx.utils.sheet_to_json(worksheet);

    try {
      await tratamento(data);
      res.json({ message: 'Arquivo Excel enviado com sucesso!' });
    } catch (error) {
      console.error(error);
      res.status(500).json({ error: 'Erro durante o processamento dos dados.' });
    }
  });
};

const tratamento = async (data) => {
  for (const element of data) {
    try {
      const result = await model.post(element);
    } catch (error) {
      throw error;
    }
  }
};



const get = (request, response) => {
    return response.status(200).json({ message: 'controller funciona' });
};

const getAll = async (request, response) => {
    const result = await model.get();
    return await response.status(200).json(result);
};

module.exports = {

  uploadFile: uploadFile,
  get: get,
  getAll: getAll

};