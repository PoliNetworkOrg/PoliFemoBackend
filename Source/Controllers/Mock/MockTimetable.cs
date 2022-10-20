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
        string jsonOrario = 
        @"
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
            }
        ]
        ";
        var orarioObj = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonOrario);
        return Ok(orarioObj);
    }
}