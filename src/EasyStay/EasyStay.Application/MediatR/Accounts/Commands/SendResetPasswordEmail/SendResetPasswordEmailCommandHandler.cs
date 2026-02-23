using EasyStay.Application.Interfaces;
using EasyStay.Application.Models.Email;
using EasyStay.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace EasyStay.Application.MediatR.Accounts.Commands.SendResetPasswordEmail;

public class SendResetPasswordEmailCommandHandler(
	UserManager<User> userManager,
	IEmailService emailService,
	IConfiguration configuration
) : IRequestHandler<SendResetPasswordEmailCommand> {

	public async Task Handle(SendResetPasswordEmailCommand request, CancellationToken cancellationToken) {
		var user = await userManager.FindByEmailAsync(request.Email);

		if (user is null)
			return;

		var resetPasswordUrlPattern = configuration["ResetPasswordUrl"]
			?? throw new NullReferenceException("ResetPasswordUrl is null");

		var token = await userManager.GeneratePasswordResetTokenAsync(user);
		var base64Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

		var resetPasswordUrl = string.Format(resetPasswordUrlPattern, user.Email, base64Token);

		var email = new EmailDto {
			Receivers = [
				new EmailReceiverDto { Name = $"{user.FirstName} {user.LastName}", Address = user.Email! }
			],
			Subject = "Відновлення паролю",
			HtmlBody = $$"""
               <!DOCTYPE html>
               <html lang="uk">
               <head>
                   <meta charset="UTF-8">
                   <meta name="viewport" content="width=device-width, initial-scale=1.0">
                   <style>
                       body {font - family: Arial, sans-serif;
                           background-color: #f4f4f4;
                           margin: 0;
                           padding: 0;
                           color: #333;
                       }
                       .container {width: 100%;
                           max-width: 600px;
                           margin: 0 auto;
                           padding: 20px;
                           background-color: #ffffff;
                           border-radius: 10px;
                           box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                       }
                       h1 {color: #444;
                           font-size: 24px;
                           margin-bottom: 20px;
                       }
                       p {line - height: 1.6;
                           margin-bottom: 20px;
                       }
                       a {display: inline-block;
                           padding: 10px 20px;
                           color: #ffffff;
                           background-color: #007bff;
                           text-decoration: none;
                           border-radius: 5px;
                           font-weight: bold;
                       }
                       a:hover {background - color: #0056b3;
                       }
                       .footer {margin - top: 20px;
                           font-size: 12px;
                           color: #999;
                       }
                   </style>
               </head>
               <body>
                   <div class="container">
                       <h1>Відновлення паролю</h1>
                       <p>Щоб змінити пароль, натисніть на кнопку нижче. Якщо ви не запитували відновлення паролю, будь ласка, ігноруйте цей лист.</p>
                       <a href="{{resetPasswordUrl}}">Відновити пароль</a>
                       <div class="footer">
                           <p>Дякуємо за використання наших послуг.</p>
                           <p>Якщо у вас виникли питання, звертайтесь до нашої підтримки.</p>
                       </div>
                   </div>
               </body>
               </html>
               """
		};

		await emailService.SendMessageAsync(email, cancellationToken);
	}
}
