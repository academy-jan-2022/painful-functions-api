using System;

namespace Codurance.FunctionAPI;

public struct NumberMessage
{
    public int Number;
    public Guid GUID;

    public NumberMessage(int number, Guid guid)
    {
        Number = number;
        GUID = guid;
    }
}
