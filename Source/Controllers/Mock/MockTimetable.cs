#region
using Microsoft.AspNetCore.Mvc;
#endregion

namespace PoliFemoBackend.Source.Controllers.Mock;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Mocks")]
[Route("v{version:apiVersion}/mock/timetable")]
[Route("/mock/timetable")]
public class MockTimetable : ControllerBase
{
    [MapToApiVersion("1.0")]
    [HttpGet]
    [Produces("application/json")]
    public ObjectResult GetMockedTimetable() 
    {
        const string jsonOrario = @"
        [
            {
                ""event_id"": 52647,
                ""date_start"": ""2022-10-20T08:15:00"",
                ""date_end"": ""2022-10-20T10:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 2074,
                    ""acronym_dn"": ""T.2.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 138464,
                ""date_start"": ""2022-10-20T08:15:00"",
                ""date_end"": ""2022-10-20T10:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 2"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 9020,
                    ""acronym_dn"": ""IV"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""005""
                }
            },
            {
                ""event_id"": 52648,
                ""date_start"": ""2022-10-20T10:15:00"",
                ""date_end"": ""2022-10-20T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 2074,
                    ""acronym_dn"": ""T.2.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 141561,
                ""date_start"": ""2022-10-21T14:15:00"",
                ""date_end"": ""2022-10-21T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 14820,
                    ""acronym_dn"": ""T.1.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 54051,
                ""date_start"": ""2022-10-24T10:15:00"",
                ""date_end"": ""2022-10-24T12:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 11207,
                    ""acronym_dn"": ""3.0.3"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""061a""
                }
            },
            {
                ""event_id"": 54053,
                ""date_start"": ""2022-10-24T12:15:00"",
                ""date_end"": ""2022-10-24T14:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 11207,
                    ""acronym_dn"": ""3.0.3"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""061a""
                }
            },
            {
                ""event_id"": 39848,
                ""date_start"": ""2022-10-24T15:15:00"",
                ""date_end"": ""2022-10-24T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE Squadra 2"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 13975,
                    ""acronym_dn"": ""6.0.1 "",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""057""
                }
            },
            {
                ""event_id"": 44934,
                ""date_start"": ""2022-10-24T15:15:00"",
                ""date_end"": ""2022-10-24T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE Squadra 1"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 7289,
                    ""acronym_dn"": ""2.0.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""030""
                }
            },
            {
                ""event_id"": 37937,
                ""date_start"": ""2022-10-25T10:15:00"",
                ""date_end"": ""2022-10-25T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 9955,
                    ""acronym_dn"": ""9.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""040""
                }
            },
            {
                ""event_id"": 76675,
                ""date_start"": ""2022-10-25T14:15:00"",
                ""date_end"": ""2022-10-25T15:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 12924,
                    ""acronym_dn"": ""B.4.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""004""
                }
            },
            {
                ""event_id"": 125427,
                ""date_start"": ""2022-10-25T14:15:00"",
                ""date_end"": ""2022-10-25T15:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 2"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 7377,
                    ""acronym_dn"": ""3.1.4"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""023""
                }
            },
            {
                ""event_id"": 134108,
                ""date_start"": ""2022-10-25T15:15:00"",
                ""date_end"": ""2022-10-25T17:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 12924,
                    ""acronym_dn"": ""B.4.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""004""
                }
            },
            {
                ""event_id"": 43473,
                ""date_start"": ""2022-10-26T08:15:00"",
                ""date_end"": ""2022-10-26T11:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 3524,
                    ""acronym_dn"": ""8.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""011""
                }
            },
            {
                ""event_id"": 43475,
                ""date_start"": ""2022-10-26T11:15:00"",
                ""date_end"": ""2022-10-26T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 3524,
                    ""acronym_dn"": ""8.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""011""
                }
            },
            {
                ""event_id"": 52679,
                ""date_start"": ""2022-10-27T08:15:00"",
                ""date_end"": ""2022-10-27T10:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 2074,
                    ""acronym_dn"": ""T.2.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 138465,
                ""date_start"": ""2022-10-27T08:15:00"",
                ""date_end"": ""2022-10-27T10:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 2"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 9020,
                    ""acronym_dn"": ""IV"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""005""
                }
            },
            {
                ""event_id"": 52680,
                ""date_start"": ""2022-10-27T10:15:00"",
                ""date_end"": ""2022-10-27T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 2074,
                    ""acronym_dn"": ""T.2.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 141563,
                ""date_start"": ""2022-10-28T14:15:00"",
                ""date_end"": ""2022-10-28T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 14820,
                    ""acronym_dn"": ""T.1.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 43497,
                ""date_start"": ""2022-11-02T08:15:00"",
                ""date_end"": ""2022-11-02T11:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 3524,
                    ""acronym_dn"": ""8.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""011""
                }
            },
            {
                ""event_id"": 43499,
                ""date_start"": ""2022-11-02T11:15:00"",
                ""date_end"": ""2022-11-02T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 3524,
                    ""acronym_dn"": ""8.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""011""
                }
            },
            {
                ""event_id"": 52697,
                ""date_start"": ""2022-11-03T08:15:00"",
                ""date_end"": ""2022-11-03T10:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 2074,
                    ""acronym_dn"": ""T.2.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 138466,
                ""date_start"": ""2022-11-03T08:15:00"",
                ""date_end"": ""2022-11-03T10:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 2"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 9020,
                    ""acronym_dn"": ""IV"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""005""
                }
            },
            {
                ""event_id"": 52698,
                ""date_start"": ""2022-11-03T10:15:00"",
                ""date_end"": ""2022-11-03T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 2074,
                    ""acronym_dn"": ""T.2.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 148784,
                ""date_start"": ""2022-11-05T11:30:00"",
                ""date_end"": ""2022-11-05T15:00:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA""
                },
                ""event_type"": {
                    ""typeId"": 2,
                    ""type_dn"": {
                        ""it"": ""Esame"",
                        ""en"": ""Exam""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 1,
                    ""calendar_dn"": {
                        ""it"": ""Esami"",
                        ""en"": ""Exams""
                    }
                }
            },
            {
                ""event_id"": 148824,
                ""date_start"": ""2022-11-07T15:00:00"",
                ""date_end"": ""2022-11-07T18:30:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1""
                },
                ""event_type"": {
                    ""typeId"": 2,
                    ""type_dn"": {
                        ""it"": ""Esame"",
                        ""en"": ""Exam""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 1,
                    ""calendar_dn"": {
                        ""it"": ""Esami"",
                        ""en"": ""Exams""
                    }
                }
            },
            {
                ""event_id"": 43536,
                ""date_start"": ""2022-11-09T08:15:00"",
                ""date_end"": ""2022-11-09T11:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 3524,
                    ""acronym_dn"": ""8.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""011""
                }
            },
            {
                ""event_id"": 43538,
                ""date_start"": ""2022-11-09T11:15:00"",
                ""date_end"": ""2022-11-09T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 3524,
                    ""acronym_dn"": ""8.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""011""
                }
            },
            {
                ""event_id"": 52729,
                ""date_start"": ""2022-11-10T08:15:00"",
                ""date_end"": ""2022-11-10T10:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 2074,
                    ""acronym_dn"": ""T.2.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 138467,
                ""date_start"": ""2022-11-10T08:15:00"",
                ""date_end"": ""2022-11-10T10:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 2"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 9020,
                    ""acronym_dn"": ""IV"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""005""
                }
            },
            {
                ""event_id"": 52730,
                ""date_start"": ""2022-11-10T10:15:00"",
                ""date_end"": ""2022-11-10T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 2074,
                    ""acronym_dn"": ""T.2.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 141567,
                ""date_start"": ""2022-11-11T14:15:00"",
                ""date_end"": ""2022-11-11T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 14820,
                    ""acronym_dn"": ""T.1.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 54151,
                ""date_start"": ""2022-11-14T10:15:00"",
                ""date_end"": ""2022-11-14T12:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 11207,
                    ""acronym_dn"": ""3.0.3"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""061a""
                }
            },
            {
                ""event_id"": 54153,
                ""date_start"": ""2022-11-14T12:15:00"",
                ""date_end"": ""2022-11-14T14:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 11207,
                    ""acronym_dn"": ""3.0.3"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""061a""
                }
            },
            {
                ""event_id"": 39928,
                ""date_start"": ""2022-11-14T15:15:00"",
                ""date_end"": ""2022-11-14T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE Squadra 2"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 13975,
                    ""acronym_dn"": ""6.0.1 "",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""057""
                }
            },
            {
                ""event_id"": 45028,
                ""date_start"": ""2022-11-14T15:15:00"",
                ""date_end"": ""2022-11-14T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE Squadra 1"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 7289,
                    ""acronym_dn"": ""2.0.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""030""
                }
            },
            {
                ""event_id"": 38040,
                ""date_start"": ""2022-11-15T10:15:00"",
                ""date_end"": ""2022-11-15T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 9955,
                    ""acronym_dn"": ""9.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""040""
                }
            },
            {
                ""event_id"": 76783,
                ""date_start"": ""2022-11-15T14:15:00"",
                ""date_end"": ""2022-11-15T15:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 12924,
                    ""acronym_dn"": ""B.4.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""004""
                }
            },
            {
                ""event_id"": 125429,
                ""date_start"": ""2022-11-15T14:15:00"",
                ""date_end"": ""2022-11-15T15:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 2"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 7377,
                    ""acronym_dn"": ""3.1.4"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""023""
                }
            },
            {
                ""event_id"": 134110,
                ""date_start"": ""2022-11-15T15:15:00"",
                ""date_end"": ""2022-11-15T17:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 12924,
                    ""acronym_dn"": ""B.4.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""004""
                }
            },
            {
                ""event_id"": 43575,
                ""date_start"": ""2022-11-16T08:15:00"",
                ""date_end"": ""2022-11-16T11:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 3524,
                    ""acronym_dn"": ""8.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""011""
                }
            },
            {
                ""event_id"": 43577,
                ""date_start"": ""2022-11-16T11:15:00"",
                ""date_end"": ""2022-11-16T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 3524,
                    ""acronym_dn"": ""8.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""011""
                }
            },
            {
                ""event_id"": 52761,
                ""date_start"": ""2022-11-17T08:15:00"",
                ""date_end"": ""2022-11-17T10:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 2074,
                    ""acronym_dn"": ""T.2.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 138468,
                ""date_start"": ""2022-11-17T08:15:00"",
                ""date_end"": ""2022-11-17T10:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 2"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 9020,
                    ""acronym_dn"": ""IV"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""005""
                }
            },
            {
                ""event_id"": 52762,
                ""date_start"": ""2022-11-17T10:15:00"",
                ""date_end"": ""2022-11-17T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 2074,
                    ""acronym_dn"": ""T.2.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 141569,
                ""date_start"": ""2022-11-18T14:15:00"",
                ""date_end"": ""2022-11-18T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 14820,
                    ""acronym_dn"": ""T.1.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 54189,
                ""date_start"": ""2022-11-21T10:15:00"",
                ""date_end"": ""2022-11-21T12:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 11207,
                    ""acronym_dn"": ""3.0.3"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""061a""
                }
            },
            {
                ""event_id"": 54191,
                ""date_start"": ""2022-11-21T12:15:00"",
                ""date_end"": ""2022-11-21T14:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 11207,
                    ""acronym_dn"": ""3.0.3"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""061a""
                }
            },
            {
                ""event_id"": 39961,
                ""date_start"": ""2022-11-21T15:15:00"",
                ""date_end"": ""2022-11-21T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE Squadra 2"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 13975,
                    ""acronym_dn"": ""6.0.1 "",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""057""
                }
            },
            {
                ""event_id"": 45066,
                ""date_start"": ""2022-11-21T15:15:00"",
                ""date_end"": ""2022-11-21T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE Squadra 1"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 7289,
                    ""acronym_dn"": ""2.0.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""030""
                }
            },
            {
                ""event_id"": 38079,
                ""date_start"": ""2022-11-22T10:15:00"",
                ""date_end"": ""2022-11-22T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 9955,
                    ""acronym_dn"": ""9.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""040""
                }
            },
            {
                ""event_id"": 76823,
                ""date_start"": ""2022-11-22T14:15:00"",
                ""date_end"": ""2022-11-22T15:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 12924,
                    ""acronym_dn"": ""B.4.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""004""
                }
            },
            {
                ""event_id"": 125430,
                ""date_start"": ""2022-11-22T14:15:00"",
                ""date_end"": ""2022-11-22T15:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 2"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 7377,
                    ""acronym_dn"": ""3.1.4"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""023""
                }
            },
            {
                ""event_id"": 134111,
                ""date_start"": ""2022-11-22T15:15:00"",
                ""date_end"": ""2022-11-22T17:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 12924,
                    ""acronym_dn"": ""B.4.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""004""
                }
            },
            {
                ""event_id"": 43614,
                ""date_start"": ""2022-11-23T08:15:00"",
                ""date_end"": ""2022-11-23T11:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 3524,
                    ""acronym_dn"": ""8.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""011""
                }
            },
            {
                ""event_id"": 43616,
                ""date_start"": ""2022-11-23T11:15:00"",
                ""date_end"": ""2022-11-23T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 3524,
                    ""acronym_dn"": ""8.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""011""
                }
            },
            {
                ""event_id"": 52793,
                ""date_start"": ""2022-11-24T08:15:00"",
                ""date_end"": ""2022-11-24T10:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 2074,
                    ""acronym_dn"": ""T.2.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 138469,
                ""date_start"": ""2022-11-24T08:15:00"",
                ""date_end"": ""2022-11-24T10:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 2"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 9020,
                    ""acronym_dn"": ""IV"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""005""
                }
            },
            {
                ""event_id"": 52794,
                ""date_start"": ""2022-11-24T10:15:00"",
                ""date_end"": ""2022-11-24T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 2074,
                    ""acronym_dn"": ""T.2.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 141571,
                ""date_start"": ""2022-11-25T14:15:00"",
                ""date_end"": ""2022-11-25T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 14820,
                    ""acronym_dn"": ""T.1.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 54227,
                ""date_start"": ""2022-11-28T10:15:00"",
                ""date_end"": ""2022-11-28T12:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 11207,
                    ""acronym_dn"": ""3.0.3"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""061a""
                }
            },
            {
                ""event_id"": 54229,
                ""date_start"": ""2022-11-28T12:15:00"",
                ""date_end"": ""2022-11-28T14:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 11207,
                    ""acronym_dn"": ""3.0.3"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""061a""
                }
            },
            {
                ""event_id"": 39994,
                ""date_start"": ""2022-11-28T15:15:00"",
                ""date_end"": ""2022-11-28T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE Squadra 2"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 13975,
                    ""acronym_dn"": ""6.0.1 "",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""057""
                }
            },
            {
                ""event_id"": 45104,
                ""date_start"": ""2022-11-28T15:15:00"",
                ""date_end"": ""2022-11-28T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE Squadra 1"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 7289,
                    ""acronym_dn"": ""2.0.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""030""
                }
            },
            {
                ""event_id"": 38118,
                ""date_start"": ""2022-11-29T10:15:00"",
                ""date_end"": ""2022-11-29T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 9955,
                    ""acronym_dn"": ""9.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""040""
                }
            },
            {
                ""event_id"": 76863,
                ""date_start"": ""2022-11-29T14:15:00"",
                ""date_end"": ""2022-11-29T15:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 12924,
                    ""acronym_dn"": ""B.4.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""004""
                }
            },
            {
                ""event_id"": 125431,
                ""date_start"": ""2022-11-29T14:15:00"",
                ""date_end"": ""2022-11-29T15:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 2"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 7377,
                    ""acronym_dn"": ""3.1.4"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""023""
                }
            },
            {
                ""event_id"": 134112,
                ""date_start"": ""2022-11-29T15:15:00"",
                ""date_end"": ""2022-11-29T17:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 12924,
                    ""acronym_dn"": ""B.4.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""004""
                }
            },
            {
                ""event_id"": 43653,
                ""date_start"": ""2022-11-30T08:15:00"",
                ""date_end"": ""2022-11-30T11:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 3524,
                    ""acronym_dn"": ""8.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""011""
                }
            },
            {
                ""event_id"": 43655,
                ""date_start"": ""2022-11-30T11:15:00"",
                ""date_end"": ""2022-11-30T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 3524,
                    ""acronym_dn"": ""8.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""011""
                }
            },
            {
                ""event_id"": 52825,
                ""date_start"": ""2022-12-01T08:15:00"",
                ""date_end"": ""2022-12-01T10:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 2074,
                    ""acronym_dn"": ""T.2.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 138470,
                ""date_start"": ""2022-12-01T08:15:00"",
                ""date_end"": ""2022-12-01T10:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 2"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 9020,
                    ""acronym_dn"": ""IV"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""005""
                }
            },
            {
                ""event_id"": 52826,
                ""date_start"": ""2022-12-01T10:15:00"",
                ""date_end"": ""2022-12-01T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 2074,
                    ""acronym_dn"": ""T.2.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 141573,
                ""date_start"": ""2022-12-02T14:15:00"",
                ""date_end"": ""2022-12-02T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 14820,
                    ""acronym_dn"": ""T.1.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 54265,
                ""date_start"": ""2022-12-05T10:15:00"",
                ""date_end"": ""2022-12-05T12:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 11207,
                    ""acronym_dn"": ""3.0.3"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""061a""
                }
            },
            {
                ""event_id"": 54267,
                ""date_start"": ""2022-12-05T12:15:00"",
                ""date_end"": ""2022-12-05T14:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 11207,
                    ""acronym_dn"": ""3.0.3"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""061a""
                }
            },
            {
                ""event_id"": 40027,
                ""date_start"": ""2022-12-05T15:15:00"",
                ""date_end"": ""2022-12-05T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE Squadra 2"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 13975,
                    ""acronym_dn"": ""6.0.1 "",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""057""
                }
            },
            {
                ""event_id"": 45142,
                ""date_start"": ""2022-12-05T15:15:00"",
                ""date_end"": ""2022-12-05T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE Squadra 1"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 7289,
                    ""acronym_dn"": ""2.0.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""030""
                }
            },
            {
                ""event_id"": 38157,
                ""date_start"": ""2022-12-06T10:15:00"",
                ""date_end"": ""2022-12-06T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 9955,
                    ""acronym_dn"": ""9.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""040""
                }
            },
            {
                ""event_id"": 76903,
                ""date_start"": ""2022-12-06T14:15:00"",
                ""date_end"": ""2022-12-06T15:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 12924,
                    ""acronym_dn"": ""B.4.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""004""
                }
            },
            {
                ""event_id"": 125432,
                ""date_start"": ""2022-12-06T14:15:00"",
                ""date_end"": ""2022-12-06T15:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 2"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 7377,
                    ""acronym_dn"": ""3.1.4"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""023""
                }
            },
            {
                ""event_id"": 134113,
                ""date_start"": ""2022-12-06T15:15:00"",
                ""date_end"": ""2022-12-06T17:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 12924,
                    ""acronym_dn"": ""B.4.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""004""
                }
            },
            {
                ""event_id"": 54279,
                ""date_start"": ""2022-12-12T10:15:00"",
                ""date_end"": ""2022-12-12T12:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 11207,
                    ""acronym_dn"": ""3.0.3"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""061a""
                }
            },
            {
                ""event_id"": 54281,
                ""date_start"": ""2022-12-12T12:15:00"",
                ""date_end"": ""2022-12-12T14:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 11207,
                    ""acronym_dn"": ""3.0.3"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""061a""
                }
            },
            {
                ""event_id"": 40046,
                ""date_start"": ""2022-12-12T15:15:00"",
                ""date_end"": ""2022-12-12T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE Squadra 2"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 13975,
                    ""acronym_dn"": ""6.0.1 "",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""057""
                }
            },
            {
                ""event_id"": 45162,
                ""date_start"": ""2022-12-12T15:15:00"",
                ""date_end"": ""2022-12-12T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE Squadra 1"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 7289,
                    ""acronym_dn"": ""2.0.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""030""
                }
            },
            {
                ""event_id"": 38171,
                ""date_start"": ""2022-12-13T10:15:00"",
                ""date_end"": ""2022-12-13T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 9955,
                    ""acronym_dn"": ""9.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""040""
                }
            },
            {
                ""event_id"": 76915,
                ""date_start"": ""2022-12-13T14:15:00"",
                ""date_end"": ""2022-12-13T15:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 12924,
                    ""acronym_dn"": ""B.4.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""004""
                }
            },
            {
                ""event_id"": 125433,
                ""date_start"": ""2022-12-13T14:15:00"",
                ""date_end"": ""2022-12-13T15:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 2"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 7377,
                    ""acronym_dn"": ""3.1.4"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""023""
                }
            },
            {
                ""event_id"": 134114,
                ""date_start"": ""2022-12-13T15:15:00"",
                ""date_end"": ""2022-12-13T17:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 12924,
                    ""acronym_dn"": ""B.4.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""004""
                }
            },
            {
                ""event_id"": 43707,
                ""date_start"": ""2022-12-14T08:15:00"",
                ""date_end"": ""2022-12-14T11:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 3524,
                    ""acronym_dn"": ""8.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""011""
                }
            },
            {
                ""event_id"": 43709,
                ""date_start"": ""2022-12-14T11:15:00"",
                ""date_end"": ""2022-12-14T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 3524,
                    ""acronym_dn"": ""8.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""011""
                }
            },
            {
                ""event_id"": 52871,
                ""date_start"": ""2022-12-15T08:15:00"",
                ""date_end"": ""2022-12-15T10:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 2074,
                    ""acronym_dn"": ""T.2.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 138471,
                ""date_start"": ""2022-12-15T08:15:00"",
                ""date_end"": ""2022-12-15T10:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1 Squadra 2"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1 Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 9020,
                    ""acronym_dn"": ""IV"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""005""
                }
            },
            {
                ""event_id"": 52872,
                ""date_start"": ""2022-12-15T10:15:00"",
                ""date_end"": ""2022-12-15T13:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 2074,
                    ""acronym_dn"": ""T.2.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 141575,
                ""date_start"": ""2022-12-16T14:15:00"",
                ""date_end"": ""2022-12-16T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 14820,
                    ""acronym_dn"": ""T.1.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""006""
                }
            },
            {
                ""event_id"": 54317,
                ""date_start"": ""2022-12-19T10:15:00"",
                ""date_end"": ""2022-12-19T12:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""FONDAMENTI DI INFORMATICA"",
                    ""en"": ""FUNDAMENTALS OF COMPUTER SCIENCE""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 11207,
                    ""acronym_dn"": ""3.0.3"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""061a""
                }
            },
            {
                ""event_id"": 54319,
                ""date_start"": ""2022-12-19T12:15:00"",
                ""date_end"": ""2022-12-19T14:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 11207,
                    ""acronym_dn"": ""3.0.3"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""061a""
                }
            },
            {
                ""event_id"": 40079,
                ""date_start"": ""2022-12-19T15:15:00"",
                ""date_end"": ""2022-12-19T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE Squadra 2"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA Team 2""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 13975,
                    ""acronym_dn"": ""6.0.1 "",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""057""
                }
            },
            {
                ""event_id"": 45200,
                ""date_start"": ""2022-12-19T15:15:00"",
                ""date_end"": ""2022-12-19T18:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""GEOMETRIA E ALGEBRA LINEARE Squadra 1"",
                    ""en"": ""GEOMETRY AND LINEAR ALGEBRA Team 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 7289,
                    ""acronym_dn"": ""2.0.2"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""030""
                }
            },
            {
                ""event_id"": 43746,
                ""date_start"": ""2022-12-21T08:15:00"",
                ""date_end"": ""2022-12-21T11:15:00"",
                ""favourite"": false,
                ""show_agenda"": true,
                ""matricola"": ""211834"",
                ""title"": {
                    ""it"": ""ANALISI MATEMATICA 1"",
                    ""en"": ""MATHEMATICAL ANALYSIS 1""
                },
                ""event_type"": {
                    ""typeId"": 1,
                    ""type_dn"": {
                        ""it"": ""Lezione"",
                        ""en"": ""Lecture""
                    }
                },
                ""calendar"": {
                    ""calendar_id"": 0,
                    ""calendar_dn"": {
                        ""it"": ""Accademico"",
                        ""en"": ""Academic""
                    }
                },
                ""room"": {
                    ""room_id"": 3524,
                    ""acronym_dn"": ""8.0.1"",
                    ""classroom_id"": -2147483648,
                    ""room_dn"": ""011""
                }
            }
        ]
        ";
        var orarioObj = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonOrario);
        return Ok(orarioObj);
    }
}