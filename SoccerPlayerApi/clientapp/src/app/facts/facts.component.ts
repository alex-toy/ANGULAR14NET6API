import { Component } from '@angular/core';
import { FactService } from '../services/fact.service';
import { GetFactsResultDto } from '../models/facts/getFactsResultDto';
import { GetFactFilterDto } from '../models/facts/GetFactFilterDto';

@Component({
  selector: 'app-facts',
  templateUrl: './facts.component.html',
  styleUrls: ['./facts.component.css']
})
export class FactsComponent {
  factsResult: GetFactsResultDto | null = null;
  errorMessage: string = '';
  filter: GetFactFilterDto = {
    type: '',
    factDimensionFilters: [],
    dimensionValueIds: []
  };

  constructor(private factService: FactService) {}

  ngOnInit(): void {
    this.fetchFacts();
  }

  fetchFacts(): void {
    this.factService.getFacts(this.filter).subscribe({
      next: (data: GetFactsResultDto) => {
        this.factsResult = data;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load facts';
        console.error(err);
      }
    });
  }

  applyFilter(): void {
    this.filter.type = 'sales';  

    // this.filter.factDimensionFilters = [
    //   {
    //     dimensionId: 101,
    //     levelId: 1,
    //     dimensionValue: 'North America'
    //   },
    //   {
    //     dimensionId: 102,
    //     levelId: 2,
    //     dimensionValue: '2025'
    //   }
    // ];

    this.filter.dimensionValueIds = [4, 2, 3];  

    this.fetchFacts();
  }
}
