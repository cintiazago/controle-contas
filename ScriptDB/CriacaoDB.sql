create database controle_contas;
use controle_contas;

create table pessoa(
	id int primary key auto_increment,
    cpf_cnpj varchar(14) not null,
    tipo_pessoa char(1) not null,
    nome varchar(80),
    razao_social varchar(80),
    nome_fantasia varchar(80),
    data_nascimento date
);

create table conta(
	id int primary key auto_increment,
	nome varchar(50) not null,
	data_criacao date not null,
	tipo_conta char(1) not null,
    id_conta_matriz int,
    saldo double,
    id_pessoa int not null,
    situacao char(1) not null,
    CONSTRAINT fk_PesConta FOREIGN KEY (id_pessoa) REFERENCES pessoa (id) 
);

create table movimentacao(
	id int primary key auto_increment,
    tipo char(1) not null,
    valor double not null,
    id_conta_debito int,
    id_conta_credito int,
    data_estorno date,
    data_movimentacao date not null,
    CONSTRAINT fk_MovContaDebito FOREIGN KEY (id_conta_debito) REFERENCES conta (id),
    CONSTRAINT fk_MovContaCredito FOREIGN KEY (id_conta_credito) REFERENCES conta (id) 
);

alter table pessoa convert to character set utf8 collate utf8_general_ci;
alter table conta convert to character set utf8 collate utf8_general_ci;
alter table movimentacao convert to character set utf8 collate utf8_general_ci;


INSERT INTO `pessoa` (`cpf_cnpj`, `tipo_pessoa`, `nome`, `razao_social`, `nome_fantasia`, `data_nascimento`)
VALUES ('077813276588', 'F', 'Cintia Zago', '', '', '1987-04-24');