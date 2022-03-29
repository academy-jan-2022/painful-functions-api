using System;

namespace Codurance.FunctionAPI;

public struct NumberMessage
{
    public int Number;
    public Guid GUID;
    public Guid CorrelationID;

    public NumberMessage(int number, Guid guid, Guid correlationID)
    {
        Number = number;
        GUID = guid;
        CorrelationID = correlationID;
    }
}
