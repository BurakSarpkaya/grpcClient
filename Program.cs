using Grpc.Net.Client;
using grpcMessageServer;
using grpcServer;

class Program
{
    static async Task Main()
    {
        //var channel = GrpcChannel.ForAddress("http://localhost:5056");
        //var greetClient = new Greeter.GreeterClient(channel);

        //HelloReply result = await greetClient.SayHelloAsync(new HelloRequest
        //{
        //    Name = "Hello Server",
        //});

        var channel = GrpcChannel.ForAddress("http://localhost:5056");
        var messageClient = new Message.MessageClient(channel);

        // unary rpc
        //MessageResponse result = await messageClient.SendMessageAsync(new MessageRequest
        //{
        //    Name = "Burak",
        //    Message = "Merhaba"
        //});

        // server streaming
        //var response = messageClient.SendMessage(new MessageRequest
        //{
        //    Name = "Burak",
        //    Message = "Merhaba"
        //});

        //CancellationTokenSource tokenSource = new CancellationTokenSource();
        //while (await response.ResponseStream.MoveNext(tokenSource.Token))
        //{
        //    Console.WriteLine(response.ResponseStream.Current.Message);
        //}

        // client streaming
        //var request = messageClient.SendMessage();

        //for (int i = 0; i<10;i++)
        //{
        //    await Task.Delay(1000);
        //    await request.RequestStream.WriteAsync(new MessageRequest
        //    {
        //        Name = "Burak",
        //        Message = "Mesaj " + i
        //    });
        //}
        //// stream datanın sonlandığını ifade eder.
        //await request.RequestStream.CompleteAsync();
        //System.Console.WriteLine((await request.ResponseAsync).Message);

        //bi-directional streaming
        var request = messageClient.SendMessage();

        var task1 = Task.Run(async () =>
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(1000);
                await request.RequestStream.WriteAsync(new MessageRequest
                {
                    Name = "Burak",
                    Message = "Mesaj " + i
                });
            }
        });

        CancellationTokenSource tokenSource = new CancellationTokenSource();
        while (await request.ResponseStream.MoveNext(tokenSource.Token))
        {
            System.Console.WriteLine(request.ResponseStream.Current.Message);
        }

        await task1;
        request.RequestStream.CompleteAsync();

    }
}

