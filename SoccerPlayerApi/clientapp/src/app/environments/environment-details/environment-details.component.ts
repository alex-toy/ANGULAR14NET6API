import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EnvironmentDto } from 'src/app/models/environments/environmentDto';
import { EnvironmentService } from 'src/app/services/environment.service';

@Component({
  selector: 'app-environment-details',
  templateUrl: './environment-details.component.html',
  styleUrls: ['./environment-details.component.css']
})
export class EnvironmentDetailsComponent {
  environment: EnvironmentDto | undefined;
  isLoading: boolean = true;

  constructor(
    private environmentService: EnvironmentService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.fetchEnvironmentDetails();
  }

  fetchEnvironmentDetails(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));  // Retrieve ID from route
    this.environmentService.getEnvironmentById(id).subscribe({
      next: (environment) => {
        this.environment = environment.data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }
}
