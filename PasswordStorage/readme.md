##Lab 5 - Password storage. Hashing.
This lab was made based on my project which is blog website. 
You can find source code [here](https://github.com/DrHopping/BlogProject/tree/Crypto) in `Crypto` branch.
Backend for this project was made with ASP.NET Core Web API and Entity Framework Core.
Frontend made with Angular.

User management implemented using ASP.NET Core Identity. 
[Default PasswordHasher](https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Extensions.Core/src/PasswordHasher.cs) for this framework uses PBKDF2 with HMAC-SHA256. 
Although this algorithm is quite reliable after reviewing [OWASP Password Storage Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html)
I decided to use `Argon2id`. Also Identity framework require to store hash and salt in one field and in default implementation there is a lot of code 
to achieve this, but in our case it's trivial task because `Argon2id` does it OOTB.

So I created [custom PasswordHasher](https://github.com/DrHopping/BlogProject/blob/Crypto/Blog/Hashing/Argon2PasswordHasher.cs) 
that uses `Sodium.Core` library and [registered](https://github.com/DrHopping/BlogProject/blob/Crypto/Blog/Extensions/ServiceExtensions.cs) it in DI container.
`Sodium.Core` implementation uses the recommended Argon2 strengths from libsodium. These don’t quite match up to the OWASP recommendations. 
However, the default "interactive" settings do roughly match:
 - OWASP: m= 37MB, t=1,p=1 or m= 15MB, t=2, p=1
 - libsodium: m= 33MB, t=4, p=1

Also before the Argon hashing added SHA3 to protect from very long passwords ddos attack.
After registering new user with this settings record in database looks like this:

![](resources\db-record.png)

Where `62/Xqn11QG6NKFfsY6ffoA` is salt and `l0eKdJq/9YkrpFrgF3sAkvhnLvawzih7nHSaK06oUJY` is digest.

To secure users from brute-force and dictionary attacks following measures were added:
1. Strong passwords requirements: 
    - Required password length - 10
    - Required lowercase
    - Required uppercase
    - Required digit
    - Required NonAlphanumeric
2. [CommonPasswordsValidator](https://github.com/DrHopping/BlogProject/blob/Crypto/Blog/Validators/CommonPasswordsValidator.cs) 
that throws error when password is in list of most common passwords.
3. Added rate-limit for authentication and user creation endpoints. Only 10 requests per 10min to Auth and only 5 per 10min to create;

