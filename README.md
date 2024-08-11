# RNet

RNet is an engine agnostic lock free multithreaded reliable udp library designed for games. RNet uses native C sockets internally for minmized latency, zig zag encoding and float quantization for low bandwidth usage. RNet is blazing fast and makes uses of native memory to ensure no gc preassure from the library at all. RNet is easily extendable and made with simplicity in mind. RNet was designed for an in production MMO and is guarenteed to stay up to date and free of vulnerabilities.

# Features
* Connection Managment
* Server to server communication
* Clients can connect to multiple servers
* Blazing fast serialization
* Custom extensions to easily add features
* Lock free thread to thread communication
* Server/Client code stripping. No server code will be included with client builds.


# Getting Started With Unity
First install the needed packages to run RNet. In the unity editor go to ```Window/Package Manager``` then click "add package from git url" in the drop down.\
\
![image](https://github.com/user-attachments/assets/aa8f8637-f77f-480e-a9d4-137ed93d5e50)



paste the following in the input box. ```https://github.com/rybakatya/RapidNet.git?path=/Unity/ENet```

Click "add package from git url" again this time pasting ```https://github.com/rybakatya/RapidNet.git?path=/Unity/RapidNetworkLibrary```

You will now see a new menu item in the unity menu. Navigate to ```RNet/SwitchTarget/Server``` to tag the build as server.


![image](https://github.com/user-attachments/assets/d1af4aa8-9cfd-409f-84f9-ccf734510c2b)


Create a new C# script called ```NetworkHandler.cs``` and paste in the following.


