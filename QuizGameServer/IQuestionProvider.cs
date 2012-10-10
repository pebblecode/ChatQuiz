namespace QuizGameServer
{
    public interface IQuestionProvider
    {
        bool TryGetQuestion(ref QuestionItem question);
    }
}