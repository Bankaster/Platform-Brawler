using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ChatManager : MonoBehaviour
{
    public TextMeshProUGUI statusText; // Pantalla verda on es mostren els missatges
    private List<string> messageLog = new List<string>(); // Llista per emmagatzemar els missatges

    private Socket clientSocket;

    void Start()
    {
        // Iniciar el fil per rebre missatges
        Thread receiveThread = new Thread(ReceiveMessages);
        receiveThread.Start();
    }

    // Mètode per establir el socket del client
    public void SetClientSocket(Socket socket)
    {
        clientSocket = socket;
    }

    // Mètode per enviar un missatge al servidor
    public void SendMessageToServer(string message)
    {
        if (!string.IsNullOrEmpty(message) && clientSocket != null)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            clientSocket.Send(data);

            // Afegir el missatge enviat a la llista i actualitzar la pantalla
            messageLog.Add("You: " + message);
            UpdateStatusText();
        }
    }

    // Mètode per rebre missatges del servidor
    void ReceiveMessages()
    {
        while (true)
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                byte[] data = new byte[1024];
                int recv = clientSocket.Receive(data);
                if (recv > 0)
                {
                    string receivedMessage = Encoding.ASCII.GetString(data, 0, recv);
                    messageLog.Add("Server: " + receivedMessage);
                    UpdateStatusText();
                }
            }
        }
    }

    // Mètode per actualitzar la pantalla verda amb els missatges
    private void UpdateStatusText()
    {
        statusText.text = ""; // Netejar el text anterior
        foreach (string msg in messageLog)
        {
            statusText.text += msg + "\n"; // Afegir els missatges a la pantalla
        }
    }
}

