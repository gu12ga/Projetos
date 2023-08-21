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

// Middleware para upload de arquivos
const uploadMiddleware = multer({ storage }).single('excelFile');

// Função para lidar com o upload de arquivos
const uploadFile = (req, res) => {
    uploadMiddleware(req, res, async (err) => {
        if (err instanceof multer.MulterError) {
            return res.status(400).json({ error: 'Erro durante o upload do arquivo.' });
        } else if (err) {
            return res.status(500).json({ error: 'Erro interno do servidor.' });
        }

        if (!req.file) {
            return res.status(400).json({ error: 'Nenhum arquivo foi enviado.' });
        }

        try {
            const data = processExcelFile(req.file.path);
            await processData(data);
            res.json({ message: 'Arquivo Excel enviado com sucesso!' });
        } catch (error) {
            console.error(error);
            res.status(500).json({ error: 'Erro durante o processamento dos dados.' });
        }
    });
};

// Função para processar arquivo Excel
const processExcelFile = (filePath) => {
    const workbook = xlsx.readFile(filePath);
    const sheetName = workbook.SheetNames[0]; // Assume que o arquivo tem apenas uma planilha
    const worksheet = workbook.Sheets[sheetName];
    return xlsx.utils.sheet_to_json(worksheet);
};

// Função para tratar os dados
const processData = async (data) => {
    for (const element of data) {
        try {
            await model.post(element);
        } catch (error) {
            throw error;
        }
    }
};

// Função para rota de teste
const get = (request, response) => {
    return response.status(200).json({ message: 'Controller funciona' });
};

// Função para obter todos os registros
const getAll = async (request, response) => {
    try {
        const result = await model.get();
        return response.status(200).json(result);
    } catch (error) {
        return response.status(500).json({ error: 'Erro ao buscar os registros.' });
    }
};

module.exports = {
    uploadFile,
    get,
    getAll
};
