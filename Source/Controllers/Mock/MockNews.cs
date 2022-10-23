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
        var jsonNews = @"
    
 [
{
""event_id"":35460,
""date_start"":""2022-10-24T10:15:00"",
""date_end"":""2022-10-24T12:15:00"",
""favourite"":false,
""show_agenda"":true,
""matricola"":""987487"",
""title"":{
            ""it"":""CHIMICA GENERALE"",
""en"":""GENERAL CHEMISTRY""
},
""event_type"":{
            ""typeId"":1,
""type_dn"":{
                ""it"":""Lezione"",
""en"":""Lecture""
}
        },
""calendar"":{
            ""calendar_id"":0,
""calendar_dn"":{
                ""it"":""Accademico"",
""en"":""Academic""
}
        },
""room"":{
            ""room_id"":12611,
""acronym_dn"":""5.1.1"",
""classroom_id"":-2147483648,
""room_dn"":""005a""
}
    },
{
""event_id"":143765,
""date_start"":""2022-10-25T08:15:00"",
""date_end"":""2022-10-25T10:15:00"",
""favourite"":false,
""show_agenda"":true,
""matricola"":""987487"",
""title"":{
""it"":""ARCHITETTURA DEI CALCOLATORI E SISTEMI OPERATIVI"",
""en"":""COMPUTER ARCHITECTURES AND OPERATING SYSTEMS""
},
""event_type"":{
""typeId"":1,
""type_dn"":{
""it"":""Lezione"",
""en"":""Lecture""
}
},
""calendar"":{
""calendar_id"":0,
""calendar_dn"":{
""it"":""Accademico"",
""en"":""Academic""
}
},
""room"":{
""room_id"":7290,
""acronym_dn"":""2.1.4"",
""classroom_id"":-2147483648,
""room_dn"":""043a""
}
},
{
""event_id"":43472,
""date_start"":""2022-10-25T15:15:00"",
""date_end"":""2022-10-25T17:15:00"",
""favourite"":false,
""show_agenda"":true,
""matricola"":""987487"",
""title"":{
""it"":""LOGICA E ALGEBRA"",
""en"":""LOGIC AND ALGEBRA""
},
""event_type"":{
""typeId"":1,
""type_dn"":{
""it"":""Lezione"",
""en"":""Lecture""
}
},
""calendar"":{
""calendar_id"":0,
""calendar_dn"":{
""it"":""Accademico"",
""en"":""Academic""
}
},
""room"":{
""room_id"":3524,
""acronym_dn"":""8.0.1"",
""classroom_id"":-2147483648,
""room_dn"":""011""
}
},
{
""event_id"":99127,
""date_start"":""2022-10-26T08:15:00"",
""date_end"":""2022-10-26T11:15:00"",
""favourite"":false,
""show_agenda"":true,
""matricola"":""987487"",
""title"":{
""it"":""ARCHITETTURA DEI CALCOLATORI E SISTEMI OPERATIVI"",
""en"":""COMPUTER ARCHITECTURES AND OPERATING SYSTEMS""
},
""event_type"":{
""typeId"":1,
""type_dn"":{
""it"":""Lezione"",
""en"":""Lecture""
}
},
""calendar"":{
""calendar_id"":0,
""calendar_dn"":{
""it"":""Accademico"",
""en"":""Academic""
}
},
""room"":{
""room_id"":6472,
""acronym_dn"":""26.16"",
""classroom_id"":-2147483648,
""room_dn"":""004c""
}
},
{
""event_id"":52674,
""date_start"":""2022-10-26T11:15:00"",
""date_end"":""2022-10-26T13:15:00"",
""favourite"":false,
""show_agenda"":true,
""matricola"":""987487"",
""title"":{
""it"":""CHIMICA GENERALE"",
""en"":""GENERAL CHEMISTRY""
},
""event_type"":{
""typeId"":1,
""type_dn"":{
""it"":""Lezione"",
""en"":""Lecture""
}
},
""calendar"":{
""calendar_id"":0,
""calendar_dn"":{
""it"":""Accademico"",
""en"":""Academic""
}
},
""room"":{
""room_id"":2074,
""acronym_dn"":""T.2.1"",
""classroom_id"":-2147483648,
""room_dn"":""006""
}
}
]";
        return Ok(jsonNews);

    }
}