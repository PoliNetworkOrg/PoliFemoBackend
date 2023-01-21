-- Uncomment the following line to enable the routine to delete old users
-- SET GLOBAL event_scheduler = ON;

-- Tables, routines and events --

create table if not exists Authors
(
    author_id int(10) auto_increment
        primary key,
    `name`     varchar(50)  null,
    link      varchar(200) null,
    image     varchar(100) null
);

create table if not exists Days
(
    `day` date not null
        primary key
);

create table if not exists Grants
(
    grant_name varchar(100) not null
        primary key
);

create table if not exists `Groups`
(
    class                    varchar(100)                                            null,
    office                   enum ('Bovisa', 'Como', 'Cremona', 'Lecco', 'Leonardo') null,
    id                       varchar(100)                                            not null
        primary key,
    degree                   enum ('LT', 'LM', 'LU')                                 null,
    school                   enum ('ICAT', 'DES', '3I', 'ICAT+3I', 'AUIC')           null,
    link_id                  varchar(50)                                             null,
    `language`                enum ('ITA', 'ENG')                                     null,
    type_                    enum ('S', 'C', 'E')                                    null,
    `year`                    varchar(10)                                             null,
    platform                 enum ('WA', 'TG', 'FB')                                 null,
    permanent_id             int                                                     null,
    last_updated             datetime                                                null,
    link_is_working          enum ('Y', 'N')                                         null
);

create table if not exists Tags
(
    `name`  varchar(100) not null
        primary key,
    image varchar(100) null
);

create table if not exists Articles
(
    article_id  int unsigned auto_increment
        primary key,
    tag_id      varchar(100)  null,
    title       varchar(100)  not null,
    subtitle    varchar(200)  null,
    content     text          not null,
    publish_time datetime      not null,
    target_time  datetime      null,
    latitude    double        null,
    longitude   double        null,
    image       varchar(200)  null,
    author_id   int(10)       not null,
    source_url   varchar(1000) null,
    constraint Author___fk
        foreign key (author_id) references Authors (author_id),
    constraint Tags___fk
        foreign key (tag_id) references Tags (`name`)
);

create table if not exists Types
(
    type_id int unsigned auto_increment
        primary key,
    `name`      enum ('Festivo', 'Esame', 'Esame di profitto', 'Lauree Magistrali', 'Lezione', 'Sabato', 'Lauree 1 liv', 'Altre attività', 'Vacanza', 'Prova in itinere') null
);

create table if not exists Users
(
    user_id       varchar(100)                               not null
        primary key,
    account_type  enum ('POLIMI', 'POLINETWORK', 'PERSONAL') not null,
    last_activity datetime                                   not null
);

create table if not exists RoomOccupancyReports
(
    room_id       int default 0 not null,
    user_id       varchar(100)  not null,
    rate          float         not null,
    when_reported datetime      not null,
    primary key (room_id, user_id),
    constraint RoomOccupancyReport_Users_user_id_fk
        foreign key (user_id) references Users (user_id)
);

create table if not exists belongsTo
(
    `day`       date         not null,
    type_id int unsigned not null,
    primary key (`day`, type_id),
    constraint belongsTo_ibfk_1
        foreign key (`day`) references Days (`day`),
    constraint belongsTo_ibfk_2
        foreign key (type_id) references Types (type_id)
);

create or replace index type_id
    on belongsTo (type_id);

create table if not exists permissions
(
    grant_id  varchar(200) not null,
    user_id   varchar(100) not null,
    object_id int          null,
    constraint grant_id
        foreign key (grant_id) references Grants (grant_name),
    constraint user_id
        foreign key (user_id) references Users (user_id)
);

create view if not exists ArticlesWithAuthors_View as
select `art`.`article_id`  AS `article_id`,
       `art`.`tag_id`      AS `tag_id`,
       `art`.`title`       AS `title`,
       `art`.`subtitle`    AS `subtitle`,
       `art`.`content`     AS `content`,
       `art`.`publish_time` AS `publish_time`,
       `art`.`target_time`  AS `target_time`,
       `art`.`latitude`    AS `latitude`,
       `art`.`longitude`   AS `longitude`,
       `art`.`image`       AS `image`,
       `art`.`author_id`   AS `author_id`,
       `art`.`source_url`   AS `source_url`,
       `aut`.`name`       AS `author_name`,
       `aut`.`link`        AS `author_link`,
       `aut`.`image`       AS `author_image`
from (`Articles` `art` join `Authors` `aut`)
where `art`.`author_id` = `aut`.`author_id`;

create
    function if not exists deleteUser(userid varchar(100)) returns int
BEGIN

    DELETE FROM permissions WHERE user_id=userid;
    DELETE FROM RoomOccupancyReports WHERE user_id=userid;
    DELETE FROM Users WHERE user_id=userid;

    RETURN 0;

END;

create
    event if not exists auto_purge_users on schedule
    every '1' DAY
        starts '2023-01-01 04:00:00'
    enable
    do
    BEGIN

    CREATE TEMPORARY TABLE IF NOT EXISTS UsersToDelete(userid VARCHAR(100));
    INSERT INTO UsersToDelete SELECT user_id from Users where last_activity < NOW() - INTERVAL 1 YEAR;
    SELECT deleteUser(userid) FROM UsersToDelete;
    DROP TABLE UsersToDelete;

END;



-- Minimal rows for the database to work --

insert ignore into Authors values(1, "Politecnico di Milano", "https://www.polimi.it/", "https://techcamp.polimi.it/wp-content/uploads/2018/11/LogoPolimi-bianco-h324.png");
insert ignore into Tags values("ALTRO", null);
insert ignore into Tags values("ATENEO", "https://www.coolinmilan.it/wp-content/uploads/2022/04/politecnico-milano-universita.jpg");
insert ignore into Tags values("RICERCA E INNOVAZIONE", "https://www.coolinmilan.it/wp-content/uploads/2022/04/politecnico-milano-universita.jpg");
insert ignore into Tags values("STUDENTI", "https://images.unsplash.com/photo-1543269865-cbf427effbad?ixlib=rb-4.0.3&auto=format&fit=crop");
