﻿namespace Test1.Exceptions
{
    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException(string message) : base(message)
        {
        }
    }
}
