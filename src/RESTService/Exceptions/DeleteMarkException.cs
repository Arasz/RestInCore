namespace RESTService.Exceptions
{
    public class DeleteMarkException : UniversityApiException
    {
        public int MarkId { get; }

        public override string Message { get; }

        public int SubjectId { get; }

        public DeleteMarkException(int markId, int subjectId, string message = "")
        {
            Message = $"Error when deleting mark {markId} in {subjectId} \n" + message;
        }
    }
}