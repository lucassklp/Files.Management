import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FilesTableService } from 'src/app/services/tables/files-table.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(public fileTable: FilesTableService, private router: Router) {

  }

  ngOnInit(): void {
    this.fileTable.update();
  }

  goToAddTemplate(){
    this.router.navigate(['/main/template/'])
  }

  goToAddMail(){
    this.router.navigate(['/main/mail/'])
  }
}
