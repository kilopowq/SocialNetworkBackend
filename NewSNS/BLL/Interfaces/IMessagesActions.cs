using DAL.Models;

namespace BLL
{
    public interface IMessagesActions
    {
        bool SendMessage(MessageDto message);

        bool PostMessage(MessageDto message);

        bool CorrectMessage(MessageDto message);

        MessageDto GetMessage(int id);
    }
}
