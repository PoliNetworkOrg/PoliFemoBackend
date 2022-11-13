create or replace table Authors
(
    id_author int(10) auto_increment
        primary key,
    name_     varchar(50)  null,
    link      varchar(200) null,
    image     varchar(100) null
);

create or replace table Days
(
    giorno date not null
        primary key
);

create or replace table Grants
(
    name_grant varchar(100) not null
        primary key
);

create or replace table `Groups`
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

create or replace table Tags
(
    name  varchar(100) not null
        primary key,
    image varchar(100) null
);

create or replace table Articles
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

create or replace table Types
(
    id_tipologia int unsigned auto_increment
        primary key,
    name         enum ('Festivo', 'Esame', 'Esame di profitto', 'Lauree Magistrali', 'Lezione', 'Sabato', 'Lauree 1 liv', 'Altre attività', 'Vacanza', 'Prova in itinere') null
);

create or replace table Users
(
    id_utente varchar(100) not null
        primary key
);

create or replace table appartiene
(
    giorno       date         not null,
    id_tipologia int unsigned not null,
    primary key (giorno, id_tipologia),
    constraint appartiene_ibfk_1
        foreign key (giorno) references Days (giorno),
    constraint appartiene_ibfk_2
        foreign key (id_tipologia) references Types (id_tipologia)
);

create or replace index id_tipologia
    on appartiene (id_tipologia);

create or replace table permission
(
    id_grant varchar(200) not null,
    id_user  varchar(100) not null,
    constraint id_grant
        foreign key (id_grant) references Grants (name_grant),
    constraint id_user
        foreign key (id_user) references Users (id_utente)
);

