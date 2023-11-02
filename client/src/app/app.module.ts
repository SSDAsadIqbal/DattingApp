import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http'; 
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { SharedModule } from './_modules/shared.module';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ErrorInterceptor } from './_interceptor/error.interceptor';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { JwtInterceptor } from './_interceptor/jwt.interceptor';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { DatePickerComponent } from './_forms/date-picker/date-picker.component';
import { CommonModule } from '@angular/common';
import { AminPanelComponent } from './admin/amin-panel/amin-panel.component';
import { HasRoleDirective } from './_directives/has-role.directive';
import { UserManagmentComponent } from './admin/user-managment/user-managment.component';
import { PhotoManagmentComponent } from './admin/photo-managment/photo-managment.component';
import { RolesModalComponent } from './modals/roles-modal/roles-modal.component';
import { RouteReuseStrategy } from '@angular/router';
import { CustomRouteReuseStratergy } from './_services/customeRouteReuseStratergy';
import { ConfirmDialogComponent } from './modals/confirm-dialog/confirm-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    ListsComponent,
    MessagesComponent,
    TestErrorsComponent,
    ServerErrorComponent,
    NotFoundComponent,
    MemberCardComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    TextInputComponent,
    DatePickerComponent,
    AminPanelComponent,
    HasRoleDirective,
    UserManagmentComponent,
    PhotoManagmentComponent,
    RolesModalComponent,
    ConfirmDialogComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,  
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    CommonModule
   
  ],
  providers: [
    {provide:HTTP_INTERCEPTORS, useClass:ErrorInterceptor, multi:true},
    {provide:HTTP_INTERCEPTORS, useClass:JwtInterceptor, multi:true},
    {provide:HTTP_INTERCEPTORS, useClass:LoadingInterceptor, multi:true},
    {provide: RouteReuseStrategy , useClass: CustomRouteReuseStratergy}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
