<<<<<<< HEAD
<<<<<<< HEAD
create
or replace table Authors
=======
create or replace table Authors
>>>>>>> dev
=======
create
or replace table Authors
>>>>>>> dev2
(
    id_author int(10) auto_increment
        primary key,
    name_     varchar(50)  null,
    link      varchar(200) null,
    image     varchar(100) null
);

<<<<<<< HEAD
<<<<<<< HEAD
create
or replace table Days
=======
create or replace table Days
>>>>>>> dev
=======
create
or replace table Days
>>>>>>> dev2
(
    giorno date not null
        primary key
);

<<<<<<< HEAD
<<<<<<< HEAD
create
or replace table Grants
=======
create or replace table Grants
>>>>>>> dev
=======
create
or replace table Grants
>>>>>>> dev2
(
    name_grant varchar(100) not null
        primary key
);

<<<<<<< HEAD
<<<<<<< HEAD
create
or replace table `Groups`
=======
create or replace table `Groups`
>>>>>>> dev
=======
create
or replace table `Groups`
>>>>>>> dev2
(
    class                    varchar(100)                                            null,
    office                   enum ('Bovisa', 'Como', 'Cremona', 'Lecco', 'Leonardo') null,
    id                       varchar(100)                                            not null
        primary key,
    degree                   enum ('LT', 'LM', 'LU')                                 null,
    school                   enum ('ICAT', 'DES', '3I', 'ICAT+3I', 'AUIC')           null,
    id_link                  varchar(50)                                             null,
    language_                enum ('ITA', 'ENG')                                     null,
    type_                    enum ('S', 'C', 'E')                                    null,
    year                     varchar(10)                                             null,
    platform                 enum ('WA', 'TG', 'FB')                                 null,
    permanent_id             int                                                     null,
    LastUpdateInviteLinkTime datetime                                                null,
    Linkfunzionante          enum ('Y', 'N')                                         null
);

<<<<<<< HEAD
<<<<<<< HEAD
create
or replace table Tags
=======
create or replace table Tags
>>>>>>> dev
=======
create
or replace table Tags
>>>>>>> dev2
(
    name  varchar(100) not null
        primary key,
    image varchar(100) null
);

<<<<<<< HEAD
<<<<<<< HEAD
create
or replace table Articles
=======
create or replace table Articles
>>>>>>> dev
=======
create
or replace table Articles
>>>>>>> dev2
(
    id_article  int unsigned auto_increment
        primary key,
    id_tag      varchar(100)  null,
    title       varchar(100)  not null,
    subtitle    varchar(200)  null,
    content     varchar(3000) not null,
    publishTime datetime      not null,
    targetTime  datetime      null,
    latitude    double        null,
    longitude   double        null,
    image       varchar(100)  null,
    id_author   int(10)       null,
    constraint Author___fk
        foreign key (id_author) references Authors (id_author),
    constraint Tags___fk
        foreign key (id_tag) references Tags (name)
);

<<<<<<< HEAD
<<<<<<< HEAD
create
or replace table Types
=======
create or replace table Types
>>>>>>> dev
=======
create
or replace table Types
>>>>>>> dev2
(
    id_tipologia int unsigned auto_increment
        primary key,
    name         enum ('Festivo', 'Esame', 'Esame di profitto', 'Lauree Magistrali', 'Lezione', 'Sabato', 'Lauree 1 liv', 'Altre attività', 'Vacanza', 'Prova in itinere') null
);

<<<<<<< HEAD
<<<<<<< HEAD
create
or replace table Users
=======
create or replace table Users
>>>>>>> dev
=======
create
or replace table Users
>>>>>>> dev2
(
    id_utente varchar(100) not null
        primary key
);

<<<<<<< HEAD
<<<<<<< HEAD
create
or replace table appartiene
=======
create or replace table appartiene
>>>>>>> dev
=======
create
or replace table appartiene
>>>>>>> dev2
(
    giorno       date         not null,
    id_tipologia int unsigned not null,
    primary key (giorno, id_tipologia),
    constraint appartiene_ibfk_1
        foreign key (giorno) references Days (giorno),
    constraint appartiene_ibfk_2
        foreign key (id_tipologia) references Types (id_tipologia)
);

<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> dev2
create
or replace index id_tipologia
    on appartiene (id_tipologia);

create
or replace table permission
<<<<<<< HEAD
=======
create or replace index id_tipologia
    on appartiene (id_tipologia);

create or replace table permission
>>>>>>> dev
=======
>>>>>>> dev2
(
    id_grant varchar(200) not null,
    id_user  varchar(100) not null,
    constraint id_grant
        foreign key (id_grant) references Grants (name_grant),
    constraint id_user
        foreign key (id_user) references Users (id_utente)
);

