#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endregion

namespace PoliFemoBackend.Source.Controllers.Mock;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Mocks")]
[Route("v{version:apiVersion}/mock/articles")]
[Route("/mock/articles")]
public class MockNews : ControllerBase
{
    [MapToApiVersion("1.0")]
    [HttpGet]
    public ObjectResult GetMockNews() 
    {
       var x = JArray.Parse("[{\"news_id\":148092,\"title\":{\"it\":\"Aree verdi: Spedizione pulitiva - Resilient G.A.P.\",\"en\":\"Green areas: Trash walk - Resilient G.A.P.\"},\"description\":{\"it\":\" < p >\r\n\t < strong > 16 ottobre 2022 </ strong >< br />\r\n\t10: 30 < br />\r\n\tOff campus San Siro </ p >\r\n < p >\r\n\tSpedizione Pulitiva nel quartiare San Siro.Un’attivit...</ p > \",\"en\":\" < p >\r\n\t < strong > 16 October 2022 </ strong >< br />\r\n\t10: 30 < br />\r\n\tOff campus San Siro </ p >\r\n < p >\r\n\tTrash walk in the San Siro neighborhood.A cleanup and trash...</ p > \"},\"text\":{\"it\":\" < p >\r\n\t < strong > 16 ottobre 2022 </ strong >< br />\r\n\t10: 30 < br />\r\n\tOff campus San Siro </ p >\r\n < p >\r\n\tSpedizione Pulitiva nel quartiare San Siro.Un’attività di pulizia e raccolta rifiuti, aperta a grandi e piccoli, per sensibilizzare sulla cura degli spazi verdi e della nostra città.La raccolta è promossa dall’associazione studentesca Resilient GAP, in collaborazione con realtà del quartiere, Fondazione Punto Sud e partnership dell’UE.Il punto di partenza sarà nella sede di Off campus, a San Siro, seguiranno ulteriori aggiornamenti sulla pagina Instagram di Resilient GAP.</ p >\r\n < p >\r\n\tPer informazioni:< br />\r\n\te - mail: < a href =\"mailto:resilientgap&#64;gmail.com\" rel=\"nofollow\">resilientgap&#64;gmail.com</a></p>\r\n\",\"en\":\"<p>\r\n\t<strong>16 October 2022 </strong><br />\r\n\t10:30<br />\r\n\tOff campus San Siro</p>\r\n<p>\r\n\tTrash walk in the San Siro neighborhood. A cleanup and trash collection activity, open to everybody, to raise awareness about caring for green spaces in our city. The collection is promoted by the student association Resilient GAP, in collaboration with neighborhood realities, &#34;Punto Sud&#34; Foundation and EU partnerships. The starting point will be at the Off campus in San Siro, more updates will follow on Resilient GAP&#39;s Instagram page.</p>\r\n<p>\r\n\tFor information:<br />\r\n\te-mail: <a href=\"mailto:resilientgap&#64;gmail.com\" rel=\"nofollow\">resilientgap&#64;gmail.com</a></p>\r\n\"},\"news_type\":{\"type_id\":0,\"type_dn\":{\"it\":\"Altro\",\"en\":\"Other\"}},\"publication_start\":\"2022-10-03\",\"publication_end\":\"2022-10-18\",\"event_start\":\"2022-10-16T00:00:00\",\"event_end\":\"2022-10-16T00:00:00\",\"favourite\":false,\"show_agenda\":false,\"tags\":[{\"event_tag_id\":43,\"denomination\":{\"it\":\"Studenti\",\"en\":\"Students\"}}],\"news_source_id\":3},{ \"news_id\":148093,\"title\":{ \"it\":\"LA sagra della matricola - Lista Aperta\",\"en\":\"LA sagra della matricola - Lista Aperta\"},\"description\":{ \"it\":\"<p>\r\n\t<strong>18 ottobre 2022</strong><br />\r\n\tdalle 18:30 alle 24:00<br />\r\n\tCampus Leonardo</p>\r\n<p>\r\n\tCome ogni anno, Lista Aperta organizza &#34;La...</p>\",\"en\":\"<p>\r\n\t<strong>18 October 2022 </strong><br />\r\n\tfrom 18:30 to 24:00<br />\r\n\tCampus Leonardo</p>\r\n<p>\r\n\tAs every year, Lista Aperta organises &#34;La sagra...</p>\"},\"text\":{ \"it\":\"<p>\r\n\t<strong>18 ottobre 2022</strong><br />\r\n\tdalle 18:30 alle 24:00<br />\r\n\tCampus Leonardo</p>\r\n<p>\r\n\tCome ogni anno, Lista Aperta organizza &#34;La sagra della matricola&#34;, un evento per accogliere le matricole al Politecnico. Quest&#39;anno inizialmente ci sarà un aperitivo con lo spritz, a seguire ceneremo e giocheremo insieme. Per concludere, ci sarà una band dal vivo di studenti del Politecnico! I posti sono limitati, compila il form e salda il contributo per iscriverti, il costo sarà di 5/10€ a testa! </p>\r\n<p>\r\n\t<a href=\"http://forms.gle/jX4Knr9BQbdo3wwBA\" rel=\"nofollow\">FORM</a></p>\r\n<p>\r\n\tPer informazioni:<br />\r\n\te-mail: <a href=\"mailto:eventi.poli.listaperta&#64;gmail.com\" rel=\"nofollow\">eventi.poli.listaperta&#64;gmail.com</a></p>\r\n\",\"en\":\"<p>\r\n\t<strong>18 October 2022 </strong><br />\r\n\tfrom 18:30 to 24:00<br />\r\n\tCampus Leonardo</p>\r\n<p>\r\n\tAs every year, Lista Aperta organises &#34;La sagra della matricola&#34;! It’s an event to welcome freshmen to Politecnico. This year we will serve spritz and crisps, dinner will follow and then we will play games together. To conclude, there will be a live band made up by Politecnico students! There will be a maximum number of people allowed, fill in the form and pay to register, the cost will be around 5/10€ each!</p>\r\n<p>\r\n\t<a href=\"http://forms.gle/jX4Knr9BQbdo3wwBA\" rel=\"nofollow\">FORM</a></p>\r\n<p>\r\n\tFor information:<br />\r\n\te-mail: <a href=\"mailto:eventi.poli.listaperta&#64;gmail.com\" rel=\"nofollow\">eventi.poli.listaperta&#64;gmail.com</a></p>\r\n\"},\"news_type\":{ \"type_id\":0,\"type_dn\":{ \"it\":\"Altro\",\"en\":\"Other\"} },\"publication_start\":\"2022-10-03\",\"publication_end\":\"2022-10-18\",\"event_start\":\"2022-10-18T00:00:00\",\"event_end\":\"2022-10-18T00:00:00\",\"favourite\":false,\"show_agenda\":false,\"tags\":[{ \"event_tag_id\":43,\"denomination\":{ \"it\":\"Studenti\",\"en\":\"Students\"} }],\"news_source_id\":3},{ \"news_id\":148107,\"title\":{ \"it\":\"Studying Abroad in China: an Immersive Experience\",\"en\":\"Studying Abroad in China: an Immersive Experience\"},\"description\":{ \"it\":\"<p>\r\n\t<strong>18 ottobre 2022<br />\r\n\t</strong>ore 18.00-19.30<br />\r\n\tEvento online</p>\r\n<p>\r\n\tL’evento è una chiacchierata informale sull...</p>\",\"en\":\"<p>\r\n\t<strong>18 October 2022</strong><br />\r\n\t6.00 pm - 7.30 pm<br />\r\n\tOnline Event</p>\r\n<p>\r\n\tThe event consists in an informal conversation on what...</p>\"},\"text\":{ \"it\":\"<p>\r\n\t<strong>18 ottobre 2022<br />\r\n\t</strong>ore 18.00-19.30<br />\r\n\tEvento online</p>\r\n<p>\r\n\tL’evento è una chiacchierata informale sull’esperienza di scambio di studi in Cina. Interverranno varie voci, tra cui docenti, ricercatori, esperti ed alumni, sia italiani che cinesi, per raccontare vari aspetti dell’esperienza immersiva di uno scambio di studi in Cina. Verrà descritto il sistema economico e universitario cinese; si parlerà delle caratteristiche e dei punti di forza delle università partner che hanno accordi di scambio con il Polimi; e si parlerà infine di come sia la vita per uno studente internazionale che si muove nell’università e nelle città cinesi. Gli studenti si porteranno a casa il “perché andare” come take away dell’evento. Questo evento è organizzato dal Dipartimento di Ingegneria Gestionale per gli studenti di Laurea Triennale e di Laurea Magistrale; si terrà in lingua inglese.</p>\r\n<p>\r\n\tPer iscriversi all’evento <strong><a href=\"https://forms.office.com/pages/responsepage.aspx?id&#61;K3EXCvNtXUKAjjCd8ope6xt02-iKhYFImSiBLKt5VFhURjFCU1k5RkZQS1FWU0tIVkkzUEJWTTVONC4u\" rel=\"nofollow\">cliccare qui</a>.</strong></p>\r\n<p>\r\n\tPer maggiori informazioni:<br />\r\n\t<a href=\"mailto:exchangemanagement-dig&#64;polimi.it\" rel=\"nofollow\">exchangemanagement-dig&#64;polimi.it</a></p>\r\n\",\"en\":\"<p>\r\n\t<strong>18 October 2022</strong><br />\r\n\t6.00 pm - 7.30 pm<br />\r\n\tOnline Event</p>\r\n<p>\r\n\tThe event consists in an informal conversation on what a study abroad experience in China is about. Numerous guests will take part in the event –professors, researchers, alumni, professionals – both of Italian and Chinese nationality, and will discuss about the many different aspects of an exchange experience in China. We will be talking about the Chinese economic and educational systems, highlight the main strengths of the Polimi partner institutions and the opportunities they offer to students; and we will also be learning about the international students’ overall life experience when studying at a Chinese institution and living in a Chinese city. The goal for students is to understand the many different reasons to choose to go to China and to live such an immersive experience. The event is organized by the Department of Management, Economics and Industrial Engineering for Bachelor and Master of Science students; it will be held in English.</p>\r\n<p>\r\n\tTo register for the event <strong><a href=\"https://forms.office.com/pages/responsepage.aspx?id&#61;K3EXCvNtXUKAjjCd8ope6xt02-iKhYFImSiBLKt5VFhURjFCU1k5RkZQS1FWU0tIVkkzUEJWTTVONC4u\" rel=\"nofollow\">click here</a>.</strong></p>\r\n<p>\r\n\tFor further information: <br />\r\n\t<a href=\"mailto:exchangemanagement-dig&#64;polimi.it\" rel=\"nofollow\">exchangemanagement-dig&#64;polimi.it</a></p>\r\n\"},\"news_type\":{ \"type_id\":0,\"type_dn\":{ \"it\":\"Altro\",\"en\":\"Other\"} },\"publication_start\":\"2022-10-03\",\"publication_end\":\"2022-10-18\",\"event_start\":\"2022-10-18T00:00:00\",\"event_end\":\"2022-10-18T00:00:00\",\"favourite\":false,\"show_agenda\":false,\"tags\":[{ \"event_tag_id\":48,\"denomination\":{ \"it\":\"Agevolazioni\",\"en\":\"Benefits\"} }],\"news_source_id\":3},{ \"news_id\":148075,\"title\":{ \"it\":\"VISIONI POLITECNICHE – Gli inventori delle città che abiteremo\",\"en\":\"VISIONI POLITECNICHE – Gli inventori delle città che abiteremo\"},\"description\":{ \"it\":\"<p>\r\n\t<strong>19 ottobre 2022</strong><br />\r\n\tore 18.00<br />\r\n\tAula Magna, Piazza Leonardo da Vinci, 32</p>\r\n<p>\r\n\t<strong>Elena Granata</strong>,</p>\",\"en\":\"<p>\r\n\t<strong>19 October 2022</strong><br />\r\n\tat 6 p.m.<br />\r\n\tAula Magna, Piazza Leonardo da Vinci, 32</p>\r\n<p>\r\n\t<strong>Elena Granata</strong><em>...</em></p>\"},\"text\":{ \"it\":\"<p>\r\n\t<strong>19 ottobre 2022</strong><br />\r\n\tore 18.00<br />\r\n\tAula Magna, Piazza Leonardo da Vinci, 32</p>\r\n<p>\r\n\t<strong>Elena Granata</strong>,<em> docente di Urbanistica e Analisi della città e del territorio <br />\r\n\t</em>Gli inventori delle città del nostro futuro non saranno più (soltanto) gli architetti. Ormai da tempo l’architettura ha perso il proprio ruolo di pungolo intelligente della società, la sua capacità di trasformazione reale dei luoghi e delle città, la sua capacità di generare visioni di lungo periodo. La rapidità dei cambiamenti in corso e le crisi che stiamo attraversando (sanitaria – climatica – geopolitica) richiedono una maggiore capacità di inventare, di immaginare e di sperimentare nuovi modelli economici e di abitare.</p>\r\n<p>\r\n\tPer informazioni:<br />\r\n\t<a href=\"http://www.eventi.polimi.it/events/gli-inventori-delle-citta-che-abiteremo/\" rel=\"nofollow\">www.eventi.polimi.it/events/gli-inventori-delle-citta-che-abiteremo/</a></p>\r\n\",\"en\":\"<p>\r\n\t<strong>19 October 2022</strong><br />\r\n\tat 6 p.m.<br />\r\n\tAula Magna, Piazza Leonardo da Vinci, 32</p>\r\n<p>\r\n\t<strong>Elena Granata</strong><em>, professor of Urban Planning and Analysis of the city and the territory.</em><br />\r\n\tThe inventors of the cities of our future will no longer (only) be architects. For some time now, architecture has lost its role as an intelligent goad of society, its capacity for real transformation of places and cities, its ability to generate long-term visions. The rapidity of the changes underway and the crises we are going through (health - climate - geopolitics) require a greater ability to invent, imagine and experiment with new economic and living models.</p>\r\n<p>\r\n\tFor information:<br />\r\n\t<a href=\"http://www.eventi.polimi.it/events/gli-inventori-delle-citta-che-abiteremo/\" rel=\"nofollow\">www.eventi.polimi.it/events/gli-inventori-delle-citta-che-abiteremo/</a></p>\r\n\"},\"news_type\":{ \"type_id\":0,\"type_dn\":{ \"it\":\"Altro\",\"en\":\"Other\"} },\"publication_start\":\"2022-10-03\",\"publication_end\":\"2022-10-18\",\"event_start\":\"2022-10-19T00:00:00\",\"event_end\":\"2022-10-19T00:00:00\",\"favourite\":false,\"show_agenda\":false,\"tags\":[{ \"event_tag_id\":5,\"denomination\":{ \"it\":\"Eventi\",\"en\":\"Events\"} }],\"news_source_id\":3},{ \"news_id\":148098,\"title\":{ \"it\":\"Viaggio in Emilia Romagna - ESN Politecnico Milano\",\"en\":\"Emilia Trip -ESN Politecnico Milano\"},\"description\":{ \"it\":\"<p>\r\n\t<strong>Dal 21 al 23 ottobre 2022</strong><br />\r\n\tEmilia Romagna</p>\r\n<p>\r\n\tSei in Italia da mesi ma non hai ancora trovato il tempo di visitare...</p>\",\"en\":\"<p>\r\n\t<strong>From 21 to 23 October 2022</strong><br />\r\n\tEmilia Romagna</p>\r\n<p>\r\n\tAre you in Italy since a while, but you haven’t found the time...</p>\"},\"text\":{ \"it\":\"<p>\r\n\t<strong>Dal 21 al 23 ottobre 2022</strong><br />\r\n\tEmilia Romagna</p>\r\n<p>\r\n\tSei in Italia da mesi ma non hai ancora trovato il tempo di visitare l’Emilia? Unisciti al nostro viaggio mozzafiato tra Modena, Bologna, Ferrara e Ravenna, alla scoperta del mondo delle Ferrari, della piadina romagnola e di tante altre curiosità storiche e culturali! Partiremo da Milano venerdì mattina presto in pullman per essere di ritorno domenica in tarda serata.</p>\r\n<p>\r\n\tPer informazioni:<br />\r\n\te-mail: <a href=\"mailto:info&#64;esnpolimi.it\" rel=\"nofollow\">info&#64;esnpolimi.it</a></p>\r\n\",\"en\":\"<p>\r\n\t<strong>From 21 to 23 October 2022</strong><br />\r\n\tEmilia Romagna</p>\r\n<p>\r\n\tAre you in Italy since a while, but you haven’t found the time to visit Emilia yet? Join our breathtaking trip around Modena, Bologna, Ferrara and Ravenna to discover the Ferrari’s world, the typical piadina and a lot of historical and cultural insights! We will leave Milan by private bus on Friday, early morning, to be back on Sunday night.</p>\r\n<p>\r\n\tFor information:<br />\r\n\te-mail: <a href=\"mailto:info&#64;esnpolimi.it\" rel=\"nofollow\">info&#64;esnpolimi.it</a></p>\r\n\"},\"news_type\":{ \"type_id\":0,\"type_dn\":{ \"it\":\"Altro\",\"en\":\"Other\"} },\"publication_start\":\"2022-10-03\",\"publication_end\":\"2022-10-18\",\"event_start\":\"2022-10-21T00:00:00\",\"event_end\":\"2022-10-23T00:00:00\",\"favourite\":false,\"show_agenda\":false,\"tags\":[{ \"event_tag_id\":43,\"denomination\":{ \"it\":\"Studenti\",\"en\":\"Students\"} }],\"news_source_id\":3},{ \"news_id\":148085,\"title\":{ \"it\":\"Fly Emotion! - Svoltastudenti\",\"en\":\"Fly Emotion! - Svoltastudenti\"},\"description\":{ \"it\":\"<p>\r\n\t<strong>23 ottobre 2022</strong><br />\r\n\tdalle 07:00 alle 19:30<br />\r\n\tFly Emotion Srl - Via San Marco 20, 23010 Albaredo per San Marco<br />\r\n\t...</p>\",\"en\":\"<p>\r\n\t<strong>23 October 2022 </strong><br />\r\n\tfrom 07:00 to 19:30<br />\r\n\tFly Emotion Srl - Via San Marco 20, 23010 Albaredo per San Marco<br />\r\n\t</p>\"},\"text\":{ \"it\":\"<p>\r\n\t<strong>23 ottobre 2022</strong><br />\r\n\tdalle 07:00 alle 19:30<br />\r\n\tFly Emotion Srl - Via San Marco 20, 23010 Albaredo per San Marco<br />\r\n\t<strong>Apertura iscrizioni: 4 Ottobre </strong></p>\r\n<p>\r\n\tAnche quest&#39;anno Svoltastudenti ti porta a &#34;volare&#34; nel fantastico paesaggio della Valtellina. L’attività che vi proponiamo è composta da 2 zipline da più di un km ciascuna che attraverseranno un&#39;incontaminata valle montana, regalando attimi di pura emozione e panorami mozzafiato. L’attività si svolgerà in coppia, ma potrete iscrivervi anche singolarmente. Le iscrizioni apriranno il 4 Ottobre alle 18.30 sul sito di Svoltastudenti. I posti sono solo 50 ed il costo di 30€ comprende anche il trasporto in pullman per Albaredo e tutte le foto ricordo in digitale dell&#39;attività!</p>\r\n<p>\r\n\tPer informazioni:<br />\r\n\t<a href=\"http://svo.lt/a-flyem\" rel=\"nofollow\">svo.lt/a-flyem</a><br />\r\n\te-mail: <a href=\"mailto:alessandromichele.digiovine&#64;svoltastudenti.it\" rel=\"nofollow\">alessandromichele.digiovine&#64;svoltastudenti.it</a>; <a href=\"mailto:elena.picchetti&#64;svoltastudenti.it\" rel=\"nofollow\">elena.picchetti&#64;svoltastudenti.it</a></p>\",\"en\":\"<p>\r\n\t<strong>23 October 2022 </strong><br />\r\n\tfrom 07:00 to 19:30<br />\r\n\tFly Emotion Srl - Via San Marco 20, 23010 Albaredo per San Marco<br />\r\n\t<strong>Registration open Oct. 4th</strong></p>\r\n<p>\r\n\tOnce again this year Svoltastudenti takes you &#34;flying&#34; in the fantastic landscape of Valtellina. The activity we propose consists of 2 ziplines of more than a kilometer each that will cross an unspoiled mountain valley, providing moments of pure excitement and breathtaking views. The activity will take place in pairs, but you can also sign up individually. Registration will open Oct. 4th at 6:30 p.m. on Svoltastudenti&#39;s website. There are only 50 available spots and the €30 cost also includes bus transportation to Albaredo and digital souvenir photos of the activity!</p>\r\n<p>\r\n\tFor information:<br />\r\n\t<a href=\"http://svo.lt/a-flyem\" rel=\"nofollow\">svo.lt/a-flyem<br />\r\n\t</a>e-mail: <a href=\"mailto:alessandromichele.digiovine&#64;svoltastudenti.it\" rel=\"nofollow\">alessandromichele.digiovine&#64;svoltastudenti.it</a>; <a href=\"mailto:elena.picchetti&#64;svoltastudenti.it\" rel=\"nofollow\">elena.picchetti&#64;svoltastudenti.it</a></p>\r\n<p>\r\n</p>\"},\"news_type\":{ \"type_id\":0,\"type_dn\":{ \"it\":\"Altro\",\"en\":\"Other\"} },\"publication_start\":\"2022-10-03\",\"publication_end\":\"2022-10-18\",\"event_start\":\"2022-10-23T00:00:00\",\"event_end\":\"2022-10-23T00:00:00\",\"favourite\":false,\"show_agenda\":false,\"tags\":[{ \"event_tag_id\":43,\"denomination\":{ \"it\":\"Studenti\",\"en\":\"Students\"} }],\"news_source_id\":3},{ \"news_id\":148079,\"title\":{ \"it\":\"Witech – Entrepreneurship for Women in Tech \",\"en\":\"Witech – Entrepreneurship for Women in Tech \"},\"description\":{ \"it\":\"<p>\r\n\t<strong>25 ottobre 2022<br />\r\n\t</strong>ore 17.00 </p>\r\n<p>\r\n\tLa conferenza ibrida, tenuta in inglese, ospiterà keynote e panel, che affronteranno...</p>\",\"en\":\"<p>\r\n\t<strong>25 October, 5pm <br />\r\n\t</strong></p>\r\n<p>\r\n\tThe hybrid conference, held in English, will host keynotes and panels, that will address different...</p>\"},\"text\":{ \"it\":\"<p>\r\n\t<strong>25 ottobre 2022<br />\r\n\t</strong>ore 17.00 </p>\r\n<p>\r\n\tLa conferenza ibrida, tenuta in inglese, ospiterà keynote e panel, che affronteranno diversi argomenti, come i programmi di educazione STEM dedicati alle ragazze, il modo in cui Diversity&amp;Inclusion viene affrontato nelle grandi aziende, l&#39;esperienza delle fondatrici di startup, le politiche e le azioni condotte dalle istituzioni per promuovere l&#39;empowerment femminile. </p>\r\n<p>\r\n\t<a href=\"https://www.eventbrite.it/e/witech-entrepreneurship-for-women-in-tech-tickets-423287754067\" rel=\"nofollow\">Iscrizioni gratuite su Eventbrite</a></p>\r\n\",\"en\":\"<p>\r\n\t<strong>25 October, 5pm <br />\r\n\t</strong></p>\r\n<p>\r\n\tThe hybrid conference, held in English, will host keynotes and panels, that will address different topics, such as STEM education programs dedicated to girls, how Diversity &amp; Inclusion is addressed in large corporations, the experience of female startup founders, policies and actions conducted by institutions to promote female empowerment. </p>\r\n<p>\r\n\t<a href=\"https://www.eventbrite.it/e/witech-entrepreneurship-for-women-in-tech-tickets-423287754067\" rel=\"nofollow\">Free registration on Eventbrite</a></p>\r\n\"},\"news_type\":{ \"type_id\":0,\"type_dn\":{ \"it\":\"Altro\",\"en\":\"Other\"} },\"publication_start\":\"2022-10-03\",\"publication_end\":\"2022-10-18\",\"event_start\":\"2022-10-25T00:00:00\",\"event_end\":\"2022-10-25T00:00:00\",\"favourite\":false,\"show_agenda\":false,\"tags\":[{ \"event_tag_id\":48,\"denomination\":{ \"it\":\"Agevolazioni\",\"en\":\"Benefits\"} }],\"news_source_id\":3},{ \"news_id\":148347,\"title\":{ \"it\":\"VISIONI POLITECNICHE – L’avanguardia dell’architettura e del design spaziale\",\"en\":\"VISIONI POLITECNICHE – L’avanguardia dell’architettura e del design spaziale\"},\"description\":{ \"it\":\"<p>\r\n\t<strong>26 ottobre 2022<br />\r\n\t</strong>ore 18.00<br />\r\n\tAula Magna, Piazza Leonardo da Vinci, 32</p>\r\n<p>\r\n\t<strong>Annalisa Dominoni e Benedetto...</strong></p>\",\"en\":\"<p>\r\n\t<strong>26 October 2022<br />\r\n\t</strong>at 6 p.m. <br />\r\n\tAula Magna, Piazza Leonardo da Vinci, 32</p>\r\n<p>\r\n\t<strong>Annalisa Dominoni</strong></p>\"},\"text\":{ \"it\":\"<p>\r\n\t<strong>26 ottobre 2022<br />\r\n\t</strong>ore 18.00<br />\r\n\tAula Magna, Piazza Leonardo da Vinci, 32</p>\r\n<p>\r\n\t<strong>Annalisa Dominoni e Benedetto Quaquaro</strong>, <em>docenti di Space Design, Politecnico di Milano<br />\r\n\t</em>Che cosa significa progettare per lo Spazio e che cosa implica? Il confinamento e la gravità ridotta sono le nuove condizioni sperimentate dagli astronauti – nella transizione che li porta sulla Stazione Spaziale Internazionale – che influenzano ogni aspetto del vivere e lavorare al di fuori della Terra. Architetti e designer sono chiamati a “dare forma” a un mondo completamente nuovo che possono solo immaginare, perché non fa parte della nostra esperienza di tutti i giorni, con l’obiettivo di incrementare le performance e il benessere dell’equipaggio.</p>\r\n<p>\r\n\tPer informazioni:<br />\r\n\t<a href=\"http://www.eventi.polimi.it/events/visioni-politecniche-lavanguardia-dellarchitettura-e-del-design-spaziale/\" rel=\"nofollow\">www.eventi.polimi.it/events/visioni-politecniche-lavanguardia-dellarchitettura-e-del-design-spaziale/</a></p>\r\n\",\"en\":\"<p>\r\n\t<strong>26 October 2022<br />\r\n\t</strong>at 6 p.m. <br />\r\n\tAula Magna, Piazza Leonardo da Vinci, 32</p>\r\n<p>\r\n\t<strong>Annalisa Dominoni</strong> and <strong>Benedetto Quaquaro</strong>, <em>professors of Space Design, Politecnico di Milano</em> <br />\r\n\tWhat does it mean to design for Space and what does it imply? Confinement and reduced gravity are the new conditions experienced by astronauts - in the transition that takes them to the International Space Station - that affect every aspect of living and working outside the Earth. Architects and designers are called to &#34;shape&#34; a completely new world that they can only imagine, because it is not part of our everyday experience, with the aim of increasing crew performance and well-being.</p>\r\n<p>\r\n\tFor information:<br />\r\n\t<a href=\"http://www.eventi.polimi.it/events/visioni-politecniche-lavanguardia-dellarchitettura-e-del-design-spaziale/\" rel=\"nofollow\">www.eventi.polimi.it/events/visioni-politecniche-lavanguardia-dellarchitettura-e-del-design-spaziale/</a></p>\r\n\"},\"news_type\":{ \"type_id\":0,\"type_dn\":{ \"it\":\"Altro\",\"en\":\"Other\"} },\"publication_start\":\"2022-10-03\",\"publication_end\":\"2022-10-18\",\"event_start\":\"2022-10-26T00:00:00\",\"event_end\":\"2022-10-26T00:00:00\",\"favourite\":false,\"show_agenda\":false,\"tags\":[{ \"event_tag_id\":5,\"denomination\":{ \"it\":\"Eventi\",\"en\":\"Events\"} }],\"news_source_id\":3},{ \"news_id\":148068,\"title\":{ \"it\":\"Andiamo al Lucca Comics&amp;Games! - Svoltastudenti\",\"en\":\"Let’s go to Lucca Comics&amp;Games - Svoltastudenti\"},\"description\":{ \"it\":\"<p>\r\n\t<strong>Dal 29 al 31 ottobre 2022</strong><br />\r\n\tLucca<br />\r\n\t<strong>Apertura iscrizioni: 3 ottobre </strong></p>\r\n<p>\r\n\tSvoltastudenti ...</p>\",\"en\":\"<p>\r\n\t<strong>From 29 to 31 October 2022</strong><br />\r\n\tLucca<br />\r\n\t<strong>Registration open: 3 October</strong></p>\r\n<p>\r\n\tSvoltastudenti takes you...</p>\"},\"text\":{ \"it\":\"<p>\r\n\t<strong>Dal 29 al 31 ottobre 2022</strong><br />\r\n\tLucca<br />\r\n\t<strong>Apertura iscrizioni: 3 ottobre </strong></p>\r\n<p>\r\n\tSvoltastudenti vi porta alla scoperta del Lucca Comics&amp;Games, la fiera internazionale dedicata al fumetto e ai videogiochi. Il festival si svolge all’interno della città storica di Lucca, tra le sue mura medievali, mentre il soggiorno avverrà a Pisa e sarà possibile visitare la città durante il primo giorno di viaggio. Il costo di 170 euro comprende: trasporto da e per Milano, biglietto due giorni per la fiera, soggiorno due notti in hotel in centro a Pisa e assicurazione. Le iscrizioni apriranno il 3 ottobre alle 18.30, i posti sono solo 53 affrettatevi!</p>\r\n<p>\r\n\tPer informazioni:<br />\r\n\ttelefono: Alessia &#43;39 338 3704379, Kekko &#43;39 388 9077047 <br />\r\n\te-mail: <a href=\"mailto:alessia.mattesini&#64;svoltastudenti.it,%20francesco.castrignano&#64;svoltastudenti.it\" rel=\"nofollow\">alessia.mattesini&#64;svoltastudenti.it, francesco.castrignano&#64;svoltastudenti.it</a><br />\r\n\t<a href=\"http://svo.lt/a-lucca\" rel=\"nofollow\">svo.lt/a-lucca</a></p>\r\n\",\"en\":\"<p>\r\n\t<strong>From 29 to 31 October 2022</strong><br />\r\n\tLucca<br />\r\n\t<strong>Registration open: 3 October</strong></p>\r\n<p>\r\n\tSvoltastudenti takes you to discover Lucca Comics&amp;Games, the international fair dedicated to comics and video games. The festival takes place within the historic city of Lucca, within its medieval walls, while the stay will take place in Pisa and it will be possible to visit the city during the first day of the trip. The cost of 170 euros includes: transportation to and from Milan, two-day ticket to the fair, two-night stay in a hotel in downtown Pisa and insurance. Registration will open on the Oct. 3 at 6:30 p.m., there are only 53 spots, so hurry up!</p>\r\n<p>\r\n\tFor information:<br />\r\n\tphone: Alessia &#43;39 338 3704379, Kekko &#43;39 388 9077047 <br />\r\n\te-mail: <a href=\"mailto:alessia.mattesini&#64;svoltastudenti.it,%20francesco.castrignano&#64;svoltastudenti.it\" rel=\"nofollow\">alessia.mattesini&#64;svoltastudenti.it, francesco.castrignano&#64;svoltastudenti.it</a><br />\r\n\t<a href=\"http://svo.lt/a-lucca\" rel=\"nofollow\">svo.lt/a-lucca</a></p>\r\n\"},\"news_type\":{ \"type_id\":0,\"type_dn\":{ \"it\":\"Altro\",\"en\":\"Other\"} },\"publication_start\":\"2022-10-03\",\"publication_end\":\"2022-10-18\",\"event_start\":\"2022-10-29T00:00:00\",\"event_end\":\"2022-10-31T00:00:00\",\"favourite\":false,\"show_agenda\":false,\"tags\":[{ \"event_tag_id\":43,\"denomination\":{ \"it\":\"Studenti\",\"en\":\"Students\"} }],\"news_source_id\":3},{ \"news_id\":148109,\"title\":{ \"it\":\"Career day PMI\",\"en\":\"Career day PMI\"},\"description\":{ \"it\":\"<p>\r\n\t<strong>16-18 novembre 2022</strong><br />\r\n\tScadenza iscrizioni: 24 ottobre 2022</p>\r\n<p>\r\n\tI dati e le tendenze parlano chiaro: le Piccole e Medie...</p>\",\"en\":\"<p>\r\n\t<strong>16-18 November 2022<br />\r\n\t</strong>Registration deadline: 24 ottobre 2022</p>\r\n<p>\r\n\tThe numbers speak: Medium Small Enterprises are one...</p>\"},\"text\":{ \"it\":\"<p>\r\n\t<strong>16-18 novembre 2022</strong><br />\r\n\tScadenza iscrizioni: 24 ottobre 2022</p>\r\n<p>\r\n\tI dati e le tendenze parlano chiaro: le Piccole e Medie Imprese in Italia costituiscono un tessuto fondamentale dell&#39;economia del nostro Paese e sono un motore verso l&#39;innovazione. Sai che circa il 60% dei neolaureati Polimi inizia a lavorare in una PMI? Il Career Day PMI è l’appuntamento esclusivo dedicato ai percorsi di crescita e alle opportunità di lavoro nelle Piccole Medie Imprese: quest’anno prenderanno parte all’evento ben 102 aziende! 3 giorni, 30/35 aziende diverse al giorno, moltissime posizioni aperte alle quali candidarti. Iscriviti all&#39;incontro con una o più aziende di tuo interesse. Una volta superato il numero massimo degli iscritti per ciascuna azienda, 90 persone, non sarà più possibile iscriversi.<strong> <br />\r\n\tApprofittane subito, h</strong><strong>ai tempo fino al 24 ottobre!</strong></p>\r\n<p>\r\n\t<a href=\"https://www.careerservice.polimi.it/it-IT/Meetings/Home/Index/?eventId&#61;29909\" rel=\"nofollow\"><strong>Scopri tutti i dettagli per partecipare sul sito del Career Service!<br />\r\n\t<br />\r\n\t</strong></a><strong><a href=\"http://www.careerservice.polimi.it/it-IT/Meetings/Home/Index/?eventId&#61;29909\" rel=\"nofollow\"> <br />\r\n\t</a></strong></p>\r\n\",\"en\":\"<p>\r\n\t<strong>16-18 November 2022<br />\r\n\t</strong>Registration deadline: 24 ottobre 2022</p>\r\n<p>\r\n\tThe numbers speak: Medium Small Enterprises are one of the most relevant businesses in the Italian economic context and they are an engine towards innovation. Do you know that almost 60% of new Polimi graduates will start to work in a SME? The Career Day PMI is a great occasion for you to discover professional paths and job opportunities. This year will take part in the fair 102 companies. 3 days: 30/35 different companies each day, many job positions you can apply for.<br />\r\n\tSign up for the meeting with one or more companies of your interest. Once the maximum number of students for each company is reached, 90 persons, it won’t be possible to register anymore. So, go ahead: <strong>have a look at the open position and apply! You have time until October 24!</strong></p>\r\n<p>\r\n\t<a href=\"https://www.careerservice.polimi.it/it-IT/Meetings/Home/Index/?eventId&#61;29909\" rel=\"nofollow\"><strong>Discover all the details on Career Service website!</strong></a><strong></strong></p>\r\n\"},\"news_type\":{ \"type_id\":0,\"type_dn\":{ \"it\":\"Altro\",\"en\":\"Other\"} },\"publication_start\":\"2022-10-03\",\"publication_end\":\"2022-10-18\",\"event_start\":\"2022-11-16T00:00:00\",\"event_end\":\"2022-11-18T00:00:00\",\"favourite\":false,\"show_agenda\":false,\"tags\":[{ \"event_tag_id\":42,\"denomination\":{ \"it\":\"Career\",\"en\":\"Career\"} }],\"news_source_id\":3},{ \"news_id\":148106,\"title\":{ \"it\":\"Avventurati nel mondo imprenditoriale olandese: Viaggio a Rotterdam &amp; Delft - Entrepreneurship Club\",\"en\":\"Discover the Dutch Startup World: Trip to Rotterdam &amp; Delft - Entrepreneurship Club\"},\"description\":{ \"it\":\"<p>\r\n\t<strong>Dal 17 al 20 novembre 2022</strong><br />\r\n\tRotterdam &amp; Delft (Olanda)<br />\r\n\t<strong>Apertura iscrizioni: 6 ottobre 2022<br />\r\n\t&lt;/...</strong></p>\",\"en\":\"<p>\r\n\t<strong>From 17th to 20th November 2022</strong><br />\r\n\tRotterdam &amp; Delft (Olanda)<br />\r\n\t<strong>Registration opens: 6th October 2022<br /></strong></p>\"},\"text\":{ \"it\":\"<p>\r\n\t<strong>Dal 17 al 20 novembre 2022</strong><br />\r\n\tRotterdam &amp; Delft (Olanda)<br />\r\n\t<strong>Apertura iscrizioni: 6 ottobre 2022<br />\r\n\t</strong><strong>Scadenza iscrizioni: 25 ottobre 2022</strong></p>\r\n<p>\r\n\tVieni con noi a Rotterdam e Delft dal 17 al 20 novembre per scoprire uno degli ecosistemi di startup più attivi e trascorrere un fine settimana nello Stato più internazionale d’Europa. Apertura delle iscrizioni: 6 ottobre alle ore 12:00. Non perdere questa opportunità, solo 30 biglietti disponibili! Più info e iscrizioni nel link qui sotto!</p>\r\n<p>\r\n\tPer informazioni:<br />\r\n\te-mail: <a href=\"mailto:info&#64;eclubpolimi.it\" rel=\"nofollow\">info&#64;eclubpolimi.it</a><br />\r\n\t<a href=\"http://forms.gle/dvaeA1bhf2qGXyLP7\" rel=\"nofollow\">forms.gle/dvaeA1bhf2qGXyLP7</a></p>\r\n\",\"en\":\"<p>\r\n\t<strong>From 17th to 20th November 2022</strong><br />\r\n\tRotterdam &amp; Delft (Olanda)<br />\r\n\t<strong>Registration opens: 6th October 2022<br />\r\n\t</strong><strong>Deadline for registration: 25 October 2022</strong></p>\r\n<p>\r\n\tCome with us to Rotterdam and Delft from 17th to 20th November to discover one of the most active startup ecosystems and to spend a weekend in the most international country in Europe. Registration opens: 6th October at 12:00. Don&#39;t miss this opportunity, only 30 tickets available! More info and registration in the link below!</p>\r\n<p>\r\n\tFor information:<br />\r\n\te-mail: <a href=\"mailto:info&#64;eclubpolimi.it\" rel=\"nofollow\">info&#64;eclubpolimi.it</a><br />\r\n\t<a href=\"http://forms.gle/dvaeA1bhf2qGXyLP7\" rel=\"nofollow\">forms.gle/dvaeA1bhf2qGXyLP7</a></p>\r\n\"},\"news_type\":{ \"type_id\":0,\"type_dn\":{ \"it\":\"Altro\",\"en\":\"Other\"} },\"publication_start\":\"2022-10-03\",\"publication_end\":\"2022-10-18\",\"event_start\":\"2022-11-17T00:00:00\",\"event_end\":\"2022-11-20T00:00:00\",\"favourite\":false,\"show_agenda\":false,\"tags\":[{ \"event_tag_id\":43,\"denomination\":{ \"it\":\"Studenti\",\"en\":\"Students\"} }],\"news_source_id\":3}]");
       return Ok(x);

    }
}