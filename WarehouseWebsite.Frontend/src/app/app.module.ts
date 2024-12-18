import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegistrationComponent } from './registration/registration.component';
import { HomeComponent } from './home/home.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {provideHttpClient, withInterceptorsFromDi} from '@angular/common/http';
import {httpInterceptorProviders} from './auth/auth-interceptor';
import { AuthTemplateComponent } from './common/auth-template/auth-template.component';
import { FooterComponent } from './common/footer/footer.component';
import { EmailInputComponent } from './common/inputs/email-input/email-input.component';
import { PasswordInputComponent } from './common/inputs/password-input/password-input.component';
import { NameInputComponent } from './common/inputs/name-input/name-input.component';
import { SurnameInputComponent } from './common/inputs/surname-input/surname-input.component'

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegistrationComponent,
    HomeComponent,
    AuthTemplateComponent,
    FooterComponent,
    EmailInputComponent,
    PasswordInputComponent,
    NameInputComponent,
    SurnameInputComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    httpInterceptorProviders,
    provideHttpClient(withInterceptorsFromDi())
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
