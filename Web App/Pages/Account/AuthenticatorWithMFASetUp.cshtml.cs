using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QRCoder;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Web_App.Data.Account;

namespace Web_App.Pages.Account
{
    [Authorize]
    public class AuthenticatorWithMFASetUpModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public SetupMFAViewModel ViewModel { get; set; }

        [BindProperty]
        public bool Succeeded { get; set; }

        public AuthenticatorWithMFASetUpModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.ViewModel = new SetupMFAViewModel();
            this.Succeeded = false;
        }

        public async Task OnGetAsync()
        {
            var user = await userManager.GetUserAsync(base.User);
            await userManager.ResetAuthenticatorKeyAsync(user);
            var key = await userManager.GetAuthenticatorKeyAsync(user);

            this.ViewModel.Key = key;
            this.ViewModel.QRCodeBytes = GenerateQRCodeBytes("my web app", key, user.Email);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid) return Page();


            var user = await userManager.GetUserAsync(base.User);

           if( await this.userManager.VerifyTwoFactorTokenAsync(
                user,
                userManager.Options.Tokens.AuthenticatorTokenProvider,
                this.ViewModel.SecurityCode
                ))
            {
               await userManager.SetTwoFactorEnabledAsync(user, true);
                this.Succeeded = true;
            }
            else
            {
                ModelState.AddModelError("AuthenticatorSetUp", "Something went wrong");
            }

           return Page();
    
        }

        private Byte[] GenerateQRCodeBytes(string provider, string key, string userEmail)
        {
            var qrCodeGenerator = new QRCodeGenerator();
            var qrCodeData =  qrCodeGenerator.CreateQrCode(
                $"otpauth://totp/{provider}:{userEmail}?secret={key}&issuer={provider}",
               QRCodeGenerator.ECCLevel.Q );

            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);

            return BitmapToByteArray(qrCodeImage);
        }

        private Byte[] BitmapToByteArray(Bitmap image)
        {
            using(MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }

        }
    }

    public class SetupMFAViewModel
    {
        public string Key { get; set; }

        public string SecurityCode { get; set; }

        public byte[] QRCodeBytes { get; set; }
    }
}
