namespace DarwinLingua.SharedKernel.Exceptions;

/// <summary>
/// Represents a domain-level validation error triggered by a violated business rule.
/// </summary>
public sealed class DomainRuleException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainRuleException"/> class.
    /// </summary>
    /// <param name="message">The domain validation message.</param>
    public DomainRuleException(string message)
        : base(message)
    {
    }
}
