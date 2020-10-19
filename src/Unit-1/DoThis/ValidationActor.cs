using Akka.Actor;

namespace WinTail
{
    class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ValidationActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                _consoleWriterActor.Tell(new NullInputError("No input received"));
            }
            else
            {
                var valid = IsValid(msg);
                if (valid)
                {
                    _consoleWriterActor.Tell(new InputSuccess("Message was ok."));
                }
                else
                {
                    _consoleWriterActor.Tell(new ValidationError("Message was not valid."));
                }

                Sender.Tell(new ContinueProcessing());
            }
        }

        private static bool IsValid(string msg)
        {
            var valid = msg.Length % 2 == 0;
            return valid;
        }
    }
}
