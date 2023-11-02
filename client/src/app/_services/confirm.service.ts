import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmDialogComponent } from '../modals/confirm-dialog/confirm-dialog.component';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
  bsModalRef?: BsModalRef<ConfirmDialogComponent>;


  constructor(private modelService: BsModalService) { }


  confirm(title = 'Confirm', 
          message = 'Are you sure you want to do this?',
          btnOk = 'Ok',
          btnCancelText = 'Cancel'
          ):Observable<boolean> {
              const config = {
                initialState: {
                title, 
                message,
                btnOk,
                btnCancelText
              }
          }
          this.bsModalRef = this.modelService.show(ConfirmDialogComponent, config);
          return this.bsModalRef.onHidden!.pipe(
            map(() => {
              return this.bsModalRef!.content!.results
            })
          )       
          }
}
