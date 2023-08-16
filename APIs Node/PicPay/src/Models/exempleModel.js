const connection = require('./connection');

const get = async () => {
    try {
        const [usuarios] = await connection.execute('SELECT * FROM Usuarios');
        console.log(usuarios);
        return usuarios;
    } catch (error) {
        console.error(error);
        throw error;
    }
};

const postT = async (data) => {
  try {
    const { payer, value, payee } = data;

    const [resultPayer] = await connection.execute('SELECT Carteira, TipoUsuario FROM Usuarios WHERE ID = ?', [payer]);
    const [resultPayee] = await connection.execute('SELECT TipoUsuario FROM Usuarios WHERE ID = ?', [payee]);

    if (resultPayer.length === 0) {
      
      return { success: false, message: 'Usuário pagador não encontrado.'};
    }

    if (resultPayee.length === 0) {

      return { success: false, message: 'Usuário recebedor não encontrado.'};

    }

    if (
      resultPayer[0].Carteira >= value &&
      resultPayer[0].TipoUsuario === 'Comum' &&
      resultPayee[0].TipoUsuario === 'Lojista'
    ) {
      await connection.execute('UPDATE Usuarios SET Carteira = ? WHERE ID = ?', [
        resultPayer[0].Carteira - value,
        payer
      ]);

      await connection.execute('INSERT INTO Transacao (IdPayer, IdPayee, payment) VALUES (?, ?, ?)', [
        payer,
        payee,
        value
      ]);

      return { success: false, message: 'Transação realizada com sucesso.'};
    } else {
      
      return { success: false, message: 'Condições para transação não atendidas.'};
    }
  } catch (error) {
    return { success: false, message: 'Erro ao processar transação:'+error.message};
  }
};

const insertNewUser = async (userData) => {

  console.log('Received userData:', userData);
  const { NomeCompleto, Carteira, CPF_CNPJ, Email, Senha, TipoUsuario } = userData;

  try {
    const sql = `
      INSERT INTO Usuarios (NomeCompleto, Carteira, CPF_CNPJ, Email, Senha, TipoUsuario)
      VALUES (?, ?, ?, ?, ?, ?)
    `;

    const [result] = await connection.execute(sql, [
      NomeCompleto,
      Carteira,
      CPF_CNPJ,
      Email,
      Senha,
      TipoUsuario
    ]);

    return { success: true, message: 'Novo usuário inserido com sucesso.', userId: result.insertId };
  } catch (error) {
    return { success: false, message: 'Erro ao inserir novo usuário.', error: error.message };
  }
};


module.exports = {

    get,
    postT,
    insertNewUser
};