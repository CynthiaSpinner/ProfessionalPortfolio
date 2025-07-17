# Home Components

This directory contains all the components that make up the home page, organized by section for better maintainability and clarity.

## Structure

```
home/
├── index.js              # Exports all home components
├── HeroSection.js        # Hero/header section with dynamic data
├── FeaturesSection.js    # Key Skills & Technologies section
├── CTASection.js         # Call-to-Action section
└── README.md            # This documentation
```

## Components

### HeroSection
- **Purpose**: Displays the main hero/header section
- **Features**: 
  - Fetches hero data from backend API
  - Real-time updates via WebSocket
  - Fallback polling if WebSocket fails
  - Loading states
- **Data Source**: `/api/portfolio/hero`

### FeaturesSection
- **Purpose**: Displays the "Key Skills & Technologies" section
- **Features**:
  - Fetches features data from backend API
  - Real-time updates via WebSocket
  - Dynamic rendering of feature cards with icons
  - Loading states
- **Data Source**: `/api/portfolio/features`

### CTASection
- **Purpose**: Displays the call-to-action section
- **Features**: Static content (can be made dynamic later if needed)

## Benefits of This Structure

1. **Separation of Concerns**: Each section has its own component with dedicated logic
2. **Maintainability**: Easy to find and modify specific sections
3. **Reusability**: Components can be reused in other pages if needed
4. **Testing**: Each component can be tested independently
5. **Performance**: Each component manages its own state and WebSocket connections
6. **Scalability**: Easy to add new sections or modify existing ones

## Usage

```javascript
import { HeroSection, FeaturesSection, CTASection } from '../components/home';

const Home = () => {
  return (
    <div className="home-page">
      <HeroSection />
      <FeaturesSection />
      <CTASection />
      <Footer />
    </div>
  );
};
```

## Adding New Sections

To add a new section:

1. Create a new component file (e.g., `NewSection.js`)
2. Add the export to `index.js`
3. Import and use in the Home page

This structure mirrors the admin dashboard organization and makes the codebase much more manageable. 