import { Component, Input } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  model:any ={};

  constructor(
    private accountService:AccountService,
    private toastr:ToastrService
    ){}

  register(){
    this.accountService.register(this.model).subscribe({
      next: () =>{
        this.cancel();
      },
      error:error => {
        this.toastr.error(error.error);
        console.log(error);
      }
        
    })
  }

  cancel() {
    alert('Registration was successful');
  }
  
}
