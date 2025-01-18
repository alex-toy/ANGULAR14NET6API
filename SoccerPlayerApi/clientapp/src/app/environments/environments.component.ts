import { Component } from '@angular/core';
import { EnvironmentService } from '../services/environment.service';
import { ResponseDto } from '../models/responseDto';
import { EnvironmentDto } from '../models/environments/environmentDto';

@Component({
  selector: 'app-environments',
  templateUrl: './environments.component.html',
  styleUrls: ['./environments.component.css']
})
export class EnvironmentsComponent {
  environments: EnvironmentDto[] = [];
  isLoading: boolean = true;
  
  constructor(
    private environmentService: EnvironmentService,
  ) {}

  ngOnInit(): void {
    this.fetchEnvironments();
  }
  
  fetchEnvironments(): void {
    this.environmentService.getEnvironments().subscribe({
      next: (response: ResponseDto<EnvironmentDto[]>) => {
        this.environments = response.data;
        console.log(this.environments)
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

}
