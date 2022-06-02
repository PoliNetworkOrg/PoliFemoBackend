drop database if exists DBPolifemo;
create database DBPolifemo;
use DBPolifemo;


/*tabella utenti*/
create table utente(
id_utente varchar(100) primary key,
ruoli int
)engine innodb;


/*tabella gruppo*/
create table gruppo(
class varchar(100),
office ENUM("Bovisa", "Como", "Cremona", "Lecco", "Leonardo"),
id varchar(100) primary key,
degree ENUM("LT", "LM", "LU"),
school ENUM("ICAT", "DES", "3I", "ICAT+3I", "AUIC"),
id_link varchar(50),
language_ ENUM("ITA", "ENG"),
type_ enum('S', 'C', 'E'), 
year varchar(10),
platform ENUM("WA", "TG", "FB"),
permanent_id int, 
LastUpdateInviteLinkTime datetime,
Linkfunzionante ENUM('Y', 'N')
)engine innodb;

/*tabella rooms*/
create table room(
  id int unsigned primary key auto_increment  NOT NULL,
  numeber int
)engine innodb;



/*tabella hashtag*/
create table hashtag(
id_hashtag int unsigned primary key auto_increment  NOT NULL,	
text_ varchar(50)
)engine innodb;

/*tabella territorial*/
create table territorial(
id_territorial int unsigned primary key auto_increment NOT NULL,	
name_ varchar(50)
)engine innodb;

/*tabella media*/
create table media(
id_media int unsigned primary key auto_increment NOT NULL
)engine innodb;

/*tabella link*/
create table link(
id_link int unsigned primary key auto_increment NOT NULL,	
link varchar(100),
id_media int unsigned,
foreign key(id_media) references media(id_media)
)engine innodb;


/*tabella POINT*/
create table point_(
id_point int unsigned primary key auto_increment NOT NULL,
latitude float,
longitude float
)engine innodb;

/*tabella LOCATION*/
create table location(
id_location int unsigned primary key auto_increment NOT NULL,	
name_ varchar(50),
link varchar(200),
id_point int unsigned,
foreign key(id_point) references point_(id_point)
)engine innodb;

/*tabella AUTHOR*/
create table author(
id_author int unsigned primary key auto_increment NOT NULL,	
name_ varchar(50),
link varchar(200)
)engine innodb;

/*tabella SOURCE*/
create table source(
id_source int unsigned primary key auto_increment NOT NULL,	
name_ varchar(50),
link varchar(200)
)engine innodb;

/*tabella article*/
create table article(
id_article int unsigned primary key auto_increment NOT NULL,	
title varchar(100),
subtitle varchar(200),
text_ varchar(1000),
publishTime datetime,
targetTime datetime,
music varchar(200),
id_media int unsigned,
foreign key(id_media) references media(id_media)	
)engine innodb;

/*tabella collega AUTHOR-ARTICLE*/
create table scritto(
id_author int unsigned,
foreign key(id_author) references author(id_author),
id_article int unsigned,
foreign key(id_article) references article(id_article)
)engine innodb;

/*tabella collega TERRITORIAL-ARTICLE*/
create table su(
id_territorial int unsigned,
foreign key(id_territorial) references territorial(id_territorial),
id_article int unsigned,
foreign key(id_article) references article(id_article)
)engine innodb;

/*tabella collega SOURCE-ARTICLE*/
create table prende(
id_source int unsigned,
foreign key(id_source) references source(id_source),
id_article int unsigned,
foreign key(id_article) references article(id_article)
)engine innodb;

/*tabella collega LOCATION-ARTICLE*/
create table where_(
id_location int unsigned,
foreign key(id_location) references location(id_location),
id_article int unsigned,
foreign key(id_article) references article(id_article)
)engine innodb;

/*tabella collega HASHTAG-ARTICLE*/
create table tag(
id_hashtag int unsigned,
foreign key(id_hashtag) references hashtag(id_hashtag),
id_article int unsigned,
foreign key(id_article) references article(id_article)
)engine innodb;


