using NwRfcNet.TypeMapper;

namespace NwRfcNet.Bapi
{
    public static class Mappper
    { 
        public static RfcMapper MapBapiReturn(this RfcMapper mapper)
        {
            mapper.Parameter<BapiReturn>()
                .Property(x => x.MessageType)
                .HasParameterName("TYPE")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(1);

            mapper.Parameter<BapiReturn>()
                .Property(x => x.Code)
                .HasParameterName("CODE")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(5);

            mapper.Parameter<BapiReturn>()
                .Property(x => x.Message)
                .HasParameterName("MESSAGE")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(220);

            mapper.Parameter<BapiReturn>()
                .Property(x => x.LogNo)
                .HasParameterName("LOG_NO")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(20);

            mapper.Parameter<BapiReturn>()
                .Property(x => x.LogMessageNumber)
                .HasParameterName("LOG_MSG_NO")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(6);

            mapper.Parameter<BapiReturn>()
                .Property(x => x.MessageV1)
                .HasParameterName("MESSAGE_V1")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(50);

            mapper.Parameter<BapiReturn>()
                .Property(x => x.MessageV2)
                .HasParameterName("MESSAGE_V2")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(50);

            mapper.Parameter<BapiReturn>()
                .Property(x => x.MessageV3)
                .HasParameterName("MESSAGE_V3")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(50);

            mapper.Parameter<BapiReturn>()
                .Property(x => x.MessageV4)
                .HasParameterName("MESSAGE_V4")
                .HasParameterType(RfcFieldType.Char)
                .MaxLength(50);

            return mapper;
        }
    }
}
